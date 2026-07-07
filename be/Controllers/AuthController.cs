using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using be.Data;
using be.DTOs;

namespace be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        // POST /api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (string.IsNullOrEmpty(dto.Username) || string.IsNullOrEmpty(dto.Password))
            {
                return BadRequest(new { message = "Username dan password wajib diisi." });
            }

            // Cari admin berdasarkan username di database
            var admin = await _context.Admins
                .FirstOrDefaultAsync(a => a.Username == dto.Username);

            // Validasi (mencocokkan password biasa sesuai isi tabel)
            if (admin == null || admin.PasswordHash != dto.Password)
            {
                return Unauthorized(new { message = "Username atau password salah!" });
            }

            // Jika sukses, kembalikan data status login ke frontend
            return Ok(new
            {
                message = "Login berhasil!",
                username = admin.Username,
                token = "MOCK_TOKEN_ADMIN_SECRET" // Token tiruan untuk disimpan di frontend
            });
        }
    }
}