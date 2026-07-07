using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using be.Data;
using be.Models;

namespace be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        // GET /api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts([FromQuery] string? search)
        {
            try
            {
                var query = _context.Products.AsQueryable();

                if (!string.IsNullOrEmpty(search))
                {
                    // Gunakan EF.Functions.Like atau pastikan ProductName tidak null
                    query = query.Where(p => p.ProductName != null && p.ProductName.Contains(search));
                }

                // Ambil semua dulu tanpa memfilter IsAvailable terlalu ketat untuk mendiagnosis data
                var products = await query.ToListAsync();
                
                return Ok(products);
            }
            catch (Exception ex)
            {
                // Jika masih error, ini akan menampilkan pesan error aslinya di Swagger/Postman agar ketahuan kerusakannya
                return StatusCode(500, $"Internal Server Error: {ex.Message} -> {ex.InnerException?.Message}");
            }
        }

        // GET /api/products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);

                if (product == null)
                {
                    return NotFound(new { message = "Menu tidak ditemukan." });
                }

                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}