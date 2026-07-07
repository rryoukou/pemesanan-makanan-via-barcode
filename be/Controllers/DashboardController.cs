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
        [HttpGet]
        [Route("stats")]
        public async Task<IActionResult> GetDashboardStats()
        {
            try
            {
                // 1. Total Revenue dari pesanan yang sudah Lunas (Beri penanganan jika null jadi 0)
                var ordersLunas = _context.Orders.Where(o => o.Status == "Lunas");
                decimal totalRevenue = await ordersLunas.AnyAsync() ? await ordersLunas.SumAsync(o => o.TotalAmount) : 0;

                // 2. Total Orders (Semua order masuk)
                var totalOrders = await _context.Orders.CountAsync();

                // 3. Product Sold (Ambil total quantity, tangani jika tabel kosong)
                var totalItems = _context.OrderDetails;
                int productSold = await totalItems.AnyAsync() ? await totalItems.SumAsync(od => od.Quantity) : 0;

                // 4. Recent Orders (5 pesanan terbaru)
                var recentOrders = await _context.Orders
                    .OrderByDescending(o => o.OrderDate)
                    .Take(5)
                    .Select(o => new
                    {
                        o.OrderId,
                        o.TableNumber,
                        o.CustomerName,
                        o.TotalAmount,
                        o.Status,
                        o.OrderDate
                    })
                    .ToListAsync();

                // 5. Data Grafik Pendapatan
                var chartQuery = await _context.Orders
                    .Where(o => o.Status == "Lunas")
                    .ToListAsync(); // Ambil ke memori dulu agar fungsi tanggal aman dari konversi SQL kuno

                var revenueChart = chartQuery
                    .GroupBy(o => o.OrderDate.Date)
                    .Select(g => new
                    {
                        Date = g.Key.ToString("yyyy-MM-dd"),
                        Revenue = g.Sum(o => o.TotalAmount)
                    })
                    .OrderBy(g => g.Date)
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
                // Kembalikan detail pesan error agar tidak menebak-nebak jika ada salah nama kolom
                return StatusCode(500, new { message = $"Internal Server Error: {ex.Message}" });
            }
        }
    }
}