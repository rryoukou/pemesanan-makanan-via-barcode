using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using be.Data;

namespace be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        // GET /api/dashboard/stats
        // revenueChart = 7 hari berurutan terakhir (termasuk hari dengan 0 pendapatan)
        [HttpGet]
        [Route("stats")]
        public async Task<IActionResult> GetDashboardStats()
        {
            try
            {
                var ordersLunas = _context.Orders.Where(o => o.Status == "Lunas");
                decimal totalRevenue = await ordersLunas.AnyAsync() ? await ordersLunas.SumAsync(o => o.TotalAmount) : 0;
                var totalOrders = await _context.Orders.CountAsync();
                var totalItems = _context.OrderDetails;
                int productSold = await totalItems.AnyAsync() ? await totalItems.SumAsync(od => od.Quantity) : 0;

                var recentOrders = await _context.Orders
                    .OrderByDescending(o => o.OrderDate)
                    .Take(5)
                    .Select(o => new { o.OrderId, o.TableNumber, o.CustomerName, o.TotalAmount, o.Status, o.OrderDate })
                    .ToListAsync();

                // Cari tanggal terakhir yang ada transaksi, lalu tampilkan 7 hari berturut-turut sebelumnya
                var allLunas = await _context.Orders
                    .Where(o => o.Status == "Lunas")
                    .ToListAsync();

                // Kelompokkan per hari
                var byDay = allLunas
                    .GroupBy(o => o.OrderDate.Date)
                    .ToDictionary(g => g.Key, g => g.Sum(o => o.TotalAmount));

                // Tentukan hari terakhir (hari terakhir ada transaksi, atau hari ini)
                var lastDay = byDay.Keys.Count > 0 ? byDay.Keys.Max() : DateTime.UtcNow.Date;

                // Buat 7 hari berurutan dari (lastDay - 6) sampai lastDay
                var dayNames = new[] { "Min", "Sen", "Sel", "Rab", "Kam", "Jum", "Sab" };
                var revenueChart = Enumerable.Range(0, 7)
                    .Select(i =>
                    {
                        var d = lastDay.AddDays(-(6 - i));
                        var dayName = dayNames[(int)d.DayOfWeek]; // Sun=0,Mon=1,...
                        return new
                        {
                            Date = d.ToString("yyyy-MM-dd"),
                            Label = $"{dayName}, {d.Day}",
                            Revenue = byDay.TryGetValue(d, out var rev) ? rev : 0
                        };
                    })
                    .ToList();

                return Ok(new
                {
                    TotalRevenue = totalRevenue,
                    TotalOrders = totalOrders,
                    ProductSold = productSold,
                    RecentOrders = recentOrders,
                    RevenueChart = revenueChart
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Internal Server Error: {ex.Message}" });
            }
        }

        // GET /api/dashboard/available-months
        [HttpGet]
        [Route("available-months")]
        public async Task<IActionResult> GetAvailableMonths()
        {
            try
            {
                var months = await _context.Orders
                    .Select(o => new { o.OrderDate.Year, o.OrderDate.Month })
                    .Distinct()
                    .OrderByDescending(m => m.Year)
                    .ThenByDescending(m => m.Month)
                    .ToListAsync();

                var monthNames = new[] {
                    "", "Januari", "Februari", "Maret", "April", "Mei", "Juni",
                    "Juli", "Agustus", "September", "Oktober", "November", "Desember"
                };

                var result = months.Select(m => new
                {
                    m.Year,
                    m.Month,
                    Label = $"{monthNames[m.Month]} {m.Year}"
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Gagal mengambil data bulan: {ex.Message}" });
            }
        }

        // GET /api/dashboard/chart?year=2026&view=yearly        → Jan-Des (12 bar)
        // GET /api/dashboard/chart?year=2026&month=7&view=weekly  → Sen-Sab (semua hari kerja)
        [HttpGet]
        [Route("chart")]
        public async Task<IActionResult> GetChartData(
            [FromQuery] int year,
            [FromQuery] int? month,
            [FromQuery] string view = "yearly")
        {
            try
            {
                var orders = await _context.Orders
                    .Where(o => o.Status == "Lunas" && o.OrderDate.Year == year)
                    .ToListAsync();

                if (view == "yearly")
                {
                    var shortMonths = new[] {
                        "", "Jan", "Feb", "Mar", "Apr", "Mei", "Jun",
                        "Jul", "Agu", "Sep", "Okt", "Nov", "Des"
                    };

                    var byMonth = orders
                        .GroupBy(o => o.OrderDate.Month)
                        .ToDictionary(g => g.Key, g => g.Sum(o => o.TotalAmount));

                    var result = Enumerable.Range(1, 12).Select(m => new
                    {
                        label = shortMonths[m],
                        revenue = byMonth.TryGetValue(m, out var rev) ? rev : 0
                    }).ToList();

                    return Ok(result);
                }
                else // weekly — Senin s.d Sabtu semua hari dalam bulan
                {
                    if (month == null)
                        return BadRequest(new { message = "Parameter 'month' wajib untuk view weekly." });

                    var monthOrders = orders
                        .Where(o => o.OrderDate.Month == month.Value)
                        .ToList();

                    var byDay = monthOrders
                        .GroupBy(o => o.OrderDate.Date)
                        .ToDictionary(g => g.Key, g => g.Sum(o => o.TotalAmount));

                    var dayLabels = new[] { "", "Sen", "Sel", "Rab", "Kam", "Jum", "Sab" };
                    var daysInMonth = DateTime.DaysInMonth(year, month.Value);

                    var allDays = Enumerable.Range(1, daysInMonth)
                        .Select(d => new DateTime(year, month.Value, d))
                        .Where(d => d.DayOfWeek != DayOfWeek.Sunday)
                        .Select(d => new
                        {
                            label = $"{dayLabels[(int)d.DayOfWeek]}, {d.Day}",
                            revenue = byDay.TryGetValue(d, out var rev) ? rev : 0
                        })
                        .ToList();

                    return Ok(allDays);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Gagal mengambil data grafik: {ex.Message}" });
            }
        }
    }
}
