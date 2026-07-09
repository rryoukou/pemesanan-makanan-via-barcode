using Microsoft.EntityFrameworkCore;
using be.Data;
using Npgsql.EntityFrameworkCore.PostgreSQL; // 👈 TAMBAHKAN BARIS INI

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection"))); // Ganti nama key connection string-nya

// 2. DAFTARKAN POLICY CORS (Izinkan Frontend Mengakses API)
builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowReactApp",
            policy =>
            {
                policy.WithOrigins(
                          "http://localhost:5173",           // Local development
                          "http://192.168.5.103:5029",       // Backend IP (untuk CORS dari client lain)
                          "https://localhost" // Dev Tunnels (backup)
                      )
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
    });

// 3. Naikkan batas ukuran upload file (default 28MB, kita set 50MB)
builder.Services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 52_428_800; // 50 MB
});
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 52_428_800; // 50 MB
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 4. AKTIFKAN STATIC FILES (untuk serve foto dari wwwroot/uploads)
app.UseStaticFiles();

// 5. AKTIFKAN MIDDLEWARE CORS (Harus dipasang SEBELUM app.MapControllers)
app.UseCors("AllowReactApp");

app.MapControllers();

app.Run();