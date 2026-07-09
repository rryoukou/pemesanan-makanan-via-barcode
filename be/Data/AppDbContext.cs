using Microsoft.EntityFrameworkCore;
using be.Models;

namespace be.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ════════════════════════════════════════════
            // 1. SEED ADMIN
            // ════════════════════════════════════════════
            modelBuilder.Entity<Admin>().HasData(
                new Admin { AdminId = 1, Username = "admin123", PasswordHash = "admin123" }
            );

            // ════════════════════════════════════════════
            // 2. SEED PRODUCTS (18 menu)
            // Kategori: Makanan Berat | Makanan Ringan | Minuman
            // ════════════════════════════════════════════
            modelBuilder.Entity<Product>().HasData(
                // ── Makanan Berat ──
                new Product { ProductId = 1,  ProductName = "Nasi Goreng Spesial",     Category = "Makanan Berat",   Price = 25000, Description = "Nasi goreng ayam plus telur mata sapi & acar",                     ImageUrl = "https://images.unsplash.com/photo-1512058564366-18510be2db19?w=600&q=80", IsAvailable = true },
                new Product { ProductId = 2,  ProductName = "Mie Goreng Jawa",         Category = "Makanan Berat",   Price = 22000, Description = "Mie goreng khas Jawa, pedas manis dengan kecap",                   ImageUrl = "https://images.unsplash.com/photo-1585032226651-759b368d7246?w=600&q=80", IsAvailable = true },
                new Product { ProductId = 3,  ProductName = "Ayam Bakar Madu",         Category = "Makanan Berat",   Price = 30000, Description = "Ayam bakar bumbu madu legit, disajikan dengan lalapan",            ImageUrl = "https://images.unsplash.com/photo-1529193591184-b1d58069ecdd?w=600&q=80", IsAvailable = true },
                new Product { ProductId = 4,  ProductName = "Nasi Uduk Komplit",       Category = "Makanan Berat",   Price = 28000, Description = "Nasi uduk dengan ayam goreng, tempe, orek tempe & sambal",         ImageUrl = "https://images.unsplash.com/photo-1567620905732-2d1ec7ab7445?w=600&q=80", IsAvailable = true },
                new Product { ProductId = 5,  ProductName = "Soto Ayam Bening",        Category = "Makanan Berat",   Price = 22000, Description = "Soto ayam kuah bening khas Jawa dengan tauge & soun",              ImageUrl = "https://images.unsplash.com/photo-1555126634-323283e090fa?w=600&q=80",    IsAvailable = true },
                new Product { ProductId = 6,  ProductName = "Ayam Geprek Keju",        Category = "Makanan Berat",   Price = 27000, Description = "Ayam geprek crispy dengan lelehan keju mozzarella panas",          ImageUrl = "https://images.unsplash.com/photo-1626082927389-6cd097cdc6ec?w=600&q=80", IsAvailable = true },
                new Product { ProductId = 7,  ProductName = "Nasi Bakar Isi Ayam",     Category = "Makanan Berat",   Price = 26000, Description = "Nasi bakar dibungkus daun pisang isi ayam suwir pedas",            ImageUrl = "https://images.unsplash.com/photo-1598515214211-89d3c73ae83b?w=600&q=80", IsAvailable = true },
                new Product { ProductId = 8,  ProductName = "Mie Kuah Tongseng",       Category = "Makanan Berat",   Price = 24000, Description = "Mie kuah dengan daging tongseng kambing & sayuran segar",          ImageUrl = "https://images.unsplash.com/photo-1569050467447-ce54b3bbc37d?w=600&q=80", IsAvailable = true },

                // ── Makanan Ringan ──
                new Product { ProductId = 9,  ProductName = "Kentang Goreng Crispy",   Category = "Makanan Ringan",  Price = 15000, Description = "Kentang goreng renyah seasoning BBQ, disajikan dengan saus",  ImageUrl = "https://images.unsplash.com/photo-1630384060421-cb20d0e0649d?w=600&q=80", IsAvailable = true },
                new Product { ProductId = 10, ProductName = "Cireng Bumbu Rujak",      Category = "Makanan Ringan",  Price = 12000, Description = "Cireng aci goreng dengan cocolan bumbu rujak pedas manis",     ImageUrl = "https://images.unsplash.com/photo-1606914707244-e398ddb0e370?w=600&q=80", IsAvailable = true },
                new Product { ProductId = 11, ProductName = "Pisang Goreng Coklat",    Category = "Makanan Ringan",  Price = 13000, Description = "Pisang goreng crispy topping coklat & keju leleh",             ImageUrl = "https://images.unsplash.com/photo-1528975604071-b4dc52a2d18c?w=600&q=80", IsAvailable = true },
                new Product { ProductId = 12, ProductName = "Tahu Crispy Sambal Ijo",  Category = "Makanan Ringan",  Price = 10000, Description = "Tahu goreng crispy disajikan dengan sambal hijau segar",        ImageUrl = "https://images.unsplash.com/photo-1546069901-ba9599a7e63c?w=600&q=80",   IsAvailable = true },
                new Product { ProductId = 13, ProductName = "Roti Bakar Nutella",      Category = "Makanan Ringan",  Price = 16000, Description = "Roti bakar tebal dengan selai Nutella & topping susu kental",   ImageUrl = "https://images.unsplash.com/photo-1484723091739-30a097e8f929?w=600&q=80", IsAvailable = true },

                // ── Minuman ──
                new Product { ProductId = 14, ProductName = "Es Teh Manis",            Category = "Minuman",         Price = 5000,  Description = "Es teh manis segar, diseduh dari teh pilihan",                 ImageUrl = "https://images.unsplash.com/photo-1556679343-c7306c1976bc?w=600&q=80",   IsAvailable = true },
                new Product { ProductId = 15, ProductName = "Jus Alpukat Shaked",      Category = "Minuman",         Price = 15000, Description = "Jus alpukat creamy dengan kerokan es krim vanilla",            ImageUrl = "https://images.unsplash.com/photo-1551023916-61b31b05de9b?w=600&q=80",   IsAvailable = true },
                new Product { ProductId = 16, ProductName = "Es Jeruk Peras",          Category = "Minuman",         Price = 8000,  Description = "Jeruk lokal segar diperas langsung, manis asam menyegarkan",   ImageUrl = "https://images.unsplash.com/photo-1621506289937-a8e4df240d0b?w=600&q=80", IsAvailable = true },
                new Product { ProductId = 17, ProductName = "Kopi Susu Gula Aren",     Category = "Minuman",         Price = 18000, Description = "Kopi robusta blend susu segar dengan gula aren pilihan",       ImageUrl = "https://images.unsplash.com/photo-1461023058943-07fcbe16d735?w=600&q=80", IsAvailable = true },
                new Product { ProductId = 18, ProductName = "Matcha Latte",            Category = "Minuman",         Price = 20000, Description = "Matcha premium Jepang dipadukan susu oat, lembut & gurih",     ImageUrl = "https://images.unsplash.com/photo-1536256263959-770b48d82b0a?w=600&q=80", IsAvailable = true }
            );


            // ════════════════════════════════════════════
            // 3. SEED ORDERS (25 transaksi — Mei s/d Juli 2026)
            // Status: "Lunas" | "Belum Bayar"
            // ════════════════════════════════════════════
            modelBuilder.Entity<Order>().HasData(
                // ── Mei 2026 (8 order) ──
                new Order { OrderId = 1,  TableNumber = "T-02", CustomerName = "Budi Santoso",    OrderDate = new DateTime(2026, 5, 3,  11, 30, 0, DateTimeKind.Utc), TotalAmount = 50000m,  Status = "Lunas",       AdditionalNotes = "Pedas semua ya" },
                new Order { OrderId = 2,  TableNumber = "T-05", CustomerName = "Siti Rahma",      OrderDate = new DateTime(2026, 5, 8,  19, 15, 0, DateTimeKind.Utc), TotalAmount = 65000m,  Status = "Lunas",       AdditionalNotes = "Minuman belakangan" },
                new Order { OrderId = 3,  TableNumber = "T-01", CustomerName = "Andi Wijaya",     OrderDate = new DateTime(2026, 5, 12, 12, 0,  0, DateTimeKind.Utc), TotalAmount = 43000m,  Status = "Lunas",       AdditionalNotes = "" },
                new Order { OrderId = 4,  TableNumber = "T-07", CustomerName = "Rina Oktavia",    OrderDate = new DateTime(2026, 5, 15, 13, 45, 0, DateTimeKind.Utc), TotalAmount = 78000m,  Status = "Lunas",       AdditionalNotes = "Tidak pedas" },
                new Order { OrderId = 5,  TableNumber = "T-03", CustomerName = "Hendra Gunawan",  OrderDate = new DateTime(2026, 5, 18, 18, 0,  0, DateTimeKind.Utc), TotalAmount = 56000m,  Status = "Lunas",       AdditionalNotes = "Ekstra sambal" },
                new Order { OrderId = 6,  TableNumber = "T-10", CustomerName = "Mega Putri",      OrderDate = new DateTime(2026, 5, 22, 20, 30, 0, DateTimeKind.Utc), TotalAmount = 48000m,  Status = "Lunas",       AdditionalNotes = "" },
                new Order { OrderId = 7,  TableNumber = "T-04", CustomerName = "Fajar Nugroho",   OrderDate = new DateTime(2026, 5, 27, 10, 15, 0, DateTimeKind.Utc), TotalAmount = 35000m,  Status = "Lunas",       AdditionalNotes = "Kurangi gula" },
                new Order { OrderId = 8,  TableNumber = "T-08", CustomerName = "Laila Sari",      OrderDate = new DateTime(2026, 5, 30, 14, 0,  0, DateTimeKind.Utc), TotalAmount = 62000m,  Status = "Lunas",       AdditionalNotes = "Pisah tagihan" },

                // ── Juni 2026 (9 order) ──
                new Order { OrderId = 9,  TableNumber = "T-06", CustomerName = "Doni Prasetyo",   OrderDate = new DateTime(2026, 6, 2,  12, 0,  0, DateTimeKind.Utc), TotalAmount = 57000m,  Status = "Lunas",       AdditionalNotes = "Ayam paha semua" },
                new Order { OrderId = 10, TableNumber = "T-02", CustomerName = "Wulandari",       OrderDate = new DateTime(2026, 6, 7,  17, 30, 0, DateTimeKind.Utc), TotalAmount = 46000m,  Status = "Lunas",       AdditionalNotes = "" },
                new Order { OrderId = 11, TableNumber = "T-09", CustomerName = "Rizky Aditya",    OrderDate = new DateTime(2026, 6, 11, 19, 0,  0, DateTimeKind.Utc), TotalAmount = 85000m,  Status = "Lunas",       AdditionalNotes = "Tambah nasi" },
                new Order { OrderId = 12, TableNumber = "T-01", CustomerName = "Putri Andini",    OrderDate = new DateTime(2026, 6, 14, 13, 0,  0, DateTimeKind.Utc), TotalAmount = 38000m,  Status = "Lunas",       AdditionalNotes = "" },
                new Order { OrderId = 13, TableNumber = "T-05", CustomerName = "Bambang Irawan",  OrderDate = new DateTime(2026, 6, 18, 11, 45, 0, DateTimeKind.Utc), TotalAmount = 70000m,  Status = "Lunas",       AdditionalNotes = "Tidak pedas sama sekali" },
                new Order { OrderId = 14, TableNumber = "T-03", CustomerName = "Nadia Fitriani",  OrderDate = new DateTime(2026, 6, 22, 20, 0,  0, DateTimeKind.Utc), TotalAmount = 55000m,  Status = "Lunas",       AdditionalNotes = "Ekstra keju" },
                new Order { OrderId = 15, TableNumber = "T-07", CustomerName = "Surya Dharma",    OrderDate = new DateTime(2026, 6, 25, 12, 30, 0, DateTimeKind.Utc), TotalAmount = 49000m,  Status = "Lunas",       AdditionalNotes = "" },
                new Order { OrderId = 16, TableNumber = "T-10", CustomerName = "Intan Permata",   OrderDate = new DateTime(2026, 6, 28, 14, 15, 0, DateTimeKind.Utc), TotalAmount = 41000m,  Status = "Lunas",       AdditionalNotes = "Tanpa bawang" },
                new Order { OrderId = 17, TableNumber = "T-04", CustomerName = "Yoga Pratama",    OrderDate = new DateTime(2026, 6, 30, 21, 0,  0, DateTimeKind.Utc), TotalAmount = 63000m,  Status = "Lunas",       AdditionalNotes = "Manis sekali" },

                // ── Juli 2026 (8 order) ──
                new Order { OrderId = 18, TableNumber = "T-06", CustomerName = "Raka Santika",    OrderDate = new DateTime(2026, 7, 1,  10, 0,  0, DateTimeKind.Utc), TotalAmount = 52000m,  Status = "Lunas",       AdditionalNotes = "" },
                new Order { OrderId = 19, TableNumber = "T-02", CustomerName = "Dewi Lestari",    OrderDate = new DateTime(2026, 7, 3,  12, 45, 0, DateTimeKind.Utc), TotalAmount = 90000m,  Status = "Lunas",       AdditionalNotes = "Ayam paha semua" },
                new Order { OrderId = 20, TableNumber = "T-08", CustomerName = "Arif Budiman",    OrderDate = new DateTime(2026, 7, 5,  19, 0,  0, DateTimeKind.Utc), TotalAmount = 74000m,  Status = "Lunas",       AdditionalNotes = "Ekstra sambal" },
                new Order { OrderId = 21, TableNumber = "T-01", CustomerName = "Sari Indah",      OrderDate = new DateTime(2026, 7, 7,  13, 30, 0, DateTimeKind.Utc), TotalAmount = 33000m,  Status = "Lunas",       AdditionalNotes = "" },
                new Order { OrderId = 22, TableNumber = "T-09", CustomerName = "Eko Baskoro",     OrderDate = new DateTime(2026, 7, 8,  18, 0,  0, DateTimeKind.Utc), TotalAmount = 59000m,  Status = "Lunas",       AdditionalNotes = "Level pedas sedang" },
                new Order { OrderId = 23, TableNumber = "T-05", CustomerName = "Rian Hidayat",    OrderDate = new DateTime(2026, 7, 9,  8,  45, 0, DateTimeKind.Utc), TotalAmount = 47000m,  Status = "Lunas",       AdditionalNotes = "Kurangi garam" },
                new Order { OrderId = 24, TableNumber = "T-03", CustomerName = "Claudia Santoso", OrderDate = new DateTime(2026, 7, 9,  14, 0,  0, DateTimeKind.Utc), TotalAmount = 38000m,  Status = "Belum Bayar", AdditionalNotes = "Tidak mau pedas" },
                new Order { OrderId = 25, TableNumber = "T-07", CustomerName = "Dimas Arya",      OrderDate = new DateTime(2026, 7, 9,  20, 15, 0, DateTimeKind.Utc), TotalAmount = 66000m,  Status = "Belum Bayar", AdditionalNotes = "Minta tisu banyak" }
            );


            // ════════════════════════════════════════════
            // 4. SEED ORDER DETAILS
            // ════════════════════════════════════════════
            modelBuilder.Entity<OrderDetail>().HasData(
                // Order 1 — Budi Santoso Rp50.000 → Nasi Goreng(2x) + Es Teh(2x)
                new OrderDetail { OrderDetailId = 1,  OrderId = 1,  ProductId = 1,  Quantity = 2, Price = 25000, Notes = "Pedas" },
                new OrderDetail { OrderDetailId = 2,  OrderId = 1,  ProductId = 14, Quantity = 2, Price = 5000,  Notes = "" },

                // Order 2 — Siti Rahma Rp65.000 → Mie Goreng(1x) + Ayam Bakar(1x) + Kopi Susu(1x) + Es Teh(1x)
                new OrderDetail { OrderDetailId = 3,  OrderId = 2,  ProductId = 2,  Quantity = 1, Price = 22000, Notes = "Sedang" },
                new OrderDetail { OrderDetailId = 4,  OrderId = 2,  ProductId = 3,  Quantity = 1, Price = 30000, Notes = "" },
                new OrderDetail { OrderDetailId = 5,  OrderId = 2,  ProductId = 17, Quantity = 1, Price = 18000, Notes = "" },
                new OrderDetail { OrderDetailId = 6,  OrderId = 2,  ProductId = 14, Quantity = 1, Price = 5000,  Notes = "Belakangan" },

                // Order 3 — Andi Wijaya Rp43.000 → Soto Ayam(1x) + Kentang Goreng(1x) + Es Jeruk(1x)
                new OrderDetail { OrderDetailId = 7,  OrderId = 3,  ProductId = 5,  Quantity = 1, Price = 22000, Notes = "" },
                new OrderDetail { OrderDetailId = 8,  OrderId = 3,  ProductId = 9,  Quantity = 1, Price = 15000, Notes = "" },
                new OrderDetail { OrderDetailId = 9,  OrderId = 3,  ProductId = 16, Quantity = 1, Price = 8000,  Notes = "Sedikit gula" },

                // Order 4 — Rina Oktavia Rp78.000 → Nasi Uduk(2x) + Matcha Latte(1x) + Es Teh(1x) + Cireng(1x)
                new OrderDetail { OrderDetailId = 10, OrderId = 4,  ProductId = 4,  Quantity = 2, Price = 28000, Notes = "Tidak pedas" },
                new OrderDetail { OrderDetailId = 11, OrderId = 4,  ProductId = 18, Quantity = 1, Price = 20000, Notes = "" },
                new OrderDetail { OrderDetailId = 12, OrderId = 4,  ProductId = 10, Quantity = 1, Price = 12000, Notes = "" },
                new OrderDetail { OrderDetailId = 13, OrderId = 4,  ProductId = 14, Quantity = 1, Price = 5000,  Notes = "" },

                // Order 5 — Hendra Gunawan Rp56.000 → Ayam Geprek(1x) + Kentang Goreng(1x) + Kopi Susu(1x) + Es Teh(1x)
                new OrderDetail { OrderDetailId = 14, OrderId = 5,  ProductId = 6,  Quantity = 1, Price = 27000, Notes = "Level 3" },
                new OrderDetail { OrderDetailId = 15, OrderId = 5,  ProductId = 9,  Quantity = 1, Price = 15000, Notes = "Ekstra saus" },
                new OrderDetail { OrderDetailId = 16, OrderId = 5,  ProductId = 17, Quantity = 1, Price = 18000, Notes = "" },
                new OrderDetail { OrderDetailId = 17, OrderId = 5,  ProductId = 14, Quantity = 1, Price = 5000,  Notes = "" },

                // Order 6 — Mega Putri Rp48.000 → Mie Kuah Tongseng(1x) + Matcha Latte(1x) + Pisang Goreng(1x)
                new OrderDetail { OrderDetailId = 18, OrderId = 6,  ProductId = 8,  Quantity = 1, Price = 24000, Notes = "" },
                new OrderDetail { OrderDetailId = 19, OrderId = 6,  ProductId = 18, Quantity = 1, Price = 20000, Notes = "" },
                new OrderDetail { OrderDetailId = 20, OrderId = 6,  ProductId = 11, Quantity = 1, Price = 13000, Notes = "Ekstra coklat" },

                // Order 7 — Fajar Nugroho Rp35.000 → Nasi Bakar(1x) + Es Teh(1x) + Tahu Crispy(1x)
                new OrderDetail { OrderDetailId = 21, OrderId = 7,  ProductId = 7,  Quantity = 1, Price = 26000, Notes = "" },
                new OrderDetail { OrderDetailId = 22, OrderId = 7,  ProductId = 14, Quantity = 1, Price = 5000,  Notes = "Kurang gula" },
                new OrderDetail { OrderDetailId = 23, OrderId = 7,  ProductId = 12, Quantity = 1, Price = 10000, Notes = "" },

                // Order 8 — Laila Sari Rp62.000 → Nasi Goreng(1x) + Ayam Bakar(1x) + Roti Bakar(1x) + Es Jeruk(1x)
                new OrderDetail { OrderDetailId = 24, OrderId = 8,  ProductId = 1,  Quantity = 1, Price = 25000, Notes = "Pedas sedang" },
                new OrderDetail { OrderDetailId = 25, OrderId = 8,  ProductId = 3,  Quantity = 1, Price = 30000, Notes = "" },
                new OrderDetail { OrderDetailId = 26, OrderId = 8,  ProductId = 13, Quantity = 1, Price = 16000, Notes = "" },
                new OrderDetail { OrderDetailId = 27, OrderId = 8,  ProductId = 16, Quantity = 1, Price = 8000,  Notes = "" },

                // Order 9 — Doni Prasetyo Rp57.000 → Ayam Bakar(1x) + Soto Ayam(1x) + Kopi Susu(1x) + Es Teh(1x)
                new OrderDetail { OrderDetailId = 28, OrderId = 9,  ProductId = 3,  Quantity = 1, Price = 30000, Notes = "Paha" },
                new OrderDetail { OrderDetailId = 29, OrderId = 9,  ProductId = 5,  Quantity = 1, Price = 22000, Notes = "" },
                new OrderDetail { OrderDetailId = 30, OrderId = 9,  ProductId = 17, Quantity = 1, Price = 18000, Notes = "" },
                new OrderDetail { OrderDetailId = 31, OrderId = 9,  ProductId = 14, Quantity = 1, Price = 5000,  Notes = "" }
            );


            modelBuilder.Entity<OrderDetail>().HasData(
                // Order 10 — Wulandari Rp46.000 → Nasi Uduk(1x) + Jus Alpukat(1x) + Cireng(1x)
                new OrderDetail { OrderDetailId = 32, OrderId = 10, ProductId = 4,  Quantity = 1, Price = 28000, Notes = "" },
                new OrderDetail { OrderDetailId = 33, OrderId = 10, ProductId = 15, Quantity = 1, Price = 15000, Notes = "" },
                new OrderDetail { OrderDetailId = 34, OrderId = 10, ProductId = 10, Quantity = 1, Price = 12000, Notes = "" },

                // Order 11 — Rizky Aditya Rp85.000 → Ayam Geprek(2x) + Kentang Goreng(2x) + Es Teh(2x) + Matcha(1x)
                new OrderDetail { OrderDetailId = 35, OrderId = 11, ProductId = 6,  Quantity = 2, Price = 27000, Notes = "Level 5" },
                new OrderDetail { OrderDetailId = 36, OrderId = 11, ProductId = 9,  Quantity = 2, Price = 15000, Notes = "" },
                new OrderDetail { OrderDetailId = 37, OrderId = 11, ProductId = 14, Quantity = 2, Price = 5000,  Notes = "" },
                new OrderDetail { OrderDetailId = 38, OrderId = 11, ProductId = 18, Quantity = 1, Price = 20000, Notes = "" },

                // Order 12 — Putri Andini Rp38.000 → Mie Goreng(1x) + Tahu Crispy(1x) + Es Jeruk(1x)
                new OrderDetail { OrderDetailId = 39, OrderId = 12, ProductId = 2,  Quantity = 1, Price = 22000, Notes = "Tidak terlalu manis" },
                new OrderDetail { OrderDetailId = 40, OrderId = 12, ProductId = 12, Quantity = 1, Price = 10000, Notes = "" },
                new OrderDetail { OrderDetailId = 41, OrderId = 12, ProductId = 16, Quantity = 1, Price = 8000,  Notes = "" },

                // Order 13 — Bambang Irawan Rp70.000 → Nasi Goreng(1x) + Ayam Bakar(1x) + Kopi Susu(1x) + Roti Bakar(1x) + Es Teh(1x)
                new OrderDetail { OrderDetailId = 42, OrderId = 13, ProductId = 1,  Quantity = 1, Price = 25000, Notes = "Tidak pedas" },
                new OrderDetail { OrderDetailId = 43, OrderId = 13, ProductId = 3,  Quantity = 1, Price = 30000, Notes = "Tidak pedas" },
                new OrderDetail { OrderDetailId = 44, OrderId = 13, ProductId = 17, Quantity = 1, Price = 18000, Notes = "" },
                new OrderDetail { OrderDetailId = 45, OrderId = 13, ProductId = 13, Quantity = 1, Price = 16000, Notes = "" },
                new OrderDetail { OrderDetailId = 46, OrderId = 13, ProductId = 14, Quantity = 1, Price = 5000,  Notes = "" },

                // Order 14 — Nadia Fitriani Rp55.000 → Ayam Geprek(1x) + Matcha(1x) + Pisang Goreng(1x)
                new OrderDetail { OrderDetailId = 47, OrderId = 14, ProductId = 6,  Quantity = 1, Price = 27000, Notes = "Ekstra keju" },
                new OrderDetail { OrderDetailId = 48, OrderId = 14, ProductId = 18, Quantity = 1, Price = 20000, Notes = "" },
                new OrderDetail { OrderDetailId = 49, OrderId = 14, ProductId = 11, Quantity = 1, Price = 13000, Notes = "Ekstra coklat" },

                // Order 15 — Surya Dharma Rp49.000 → Soto Ayam(1x) + Nasi Bakar(1x) + Kopi Susu(1x)
                new OrderDetail { OrderDetailId = 50, OrderId = 15, ProductId = 5,  Quantity = 1, Price = 22000, Notes = "" },
                new OrderDetail { OrderDetailId = 51, OrderId = 15, ProductId = 7,  Quantity = 1, Price = 26000, Notes = "" },
                new OrderDetail { OrderDetailId = 52, OrderId = 15, ProductId = 17, Quantity = 1, Price = 18000, Notes = "" },

                // Order 16 — Intan Permata Rp41.000 → Mie Kuah Tongseng(1x) + Es Teh(1x) + Cireng(1x)
                new OrderDetail { OrderDetailId = 53, OrderId = 16, ProductId = 8,  Quantity = 1, Price = 24000, Notes = "Tanpa bawang" },
                new OrderDetail { OrderDetailId = 54, OrderId = 16, ProductId = 14, Quantity = 1, Price = 5000,  Notes = "" },
                new OrderDetail { OrderDetailId = 55, OrderId = 16, ProductId = 10, Quantity = 1, Price = 12000, Notes = "" },

                // Order 17 — Yoga Pratama Rp63.000 → Nasi Uduk(1x) + Ayam Geprek(1x) + Jus Alpukat(1x) + Tahu Crispy(1x)
                new OrderDetail { OrderDetailId = 56, OrderId = 17, ProductId = 4,  Quantity = 1, Price = 28000, Notes = "" },
                new OrderDetail { OrderDetailId = 57, OrderId = 17, ProductId = 6,  Quantity = 1, Price = 27000, Notes = "Sangat manis" },
                new OrderDetail { OrderDetailId = 58, OrderId = 17, ProductId = 15, Quantity = 1, Price = 15000, Notes = "" },
                new OrderDetail { OrderDetailId = 59, OrderId = 17, ProductId = 12, Quantity = 1, Price = 10000, Notes = "" }
            );


            modelBuilder.Entity<OrderDetail>().HasData(
                // Order 18 — Raka Santika Rp52.000 → Nasi Goreng(1x) + Soto Ayam(1x) + Matcha Latte(1x)
                new OrderDetail { OrderDetailId = 60, OrderId = 18, ProductId = 1,  Quantity = 1, Price = 25000, Notes = "" },
                new OrderDetail { OrderDetailId = 61, OrderId = 18, ProductId = 5,  Quantity = 1, Price = 22000, Notes = "" },
                new OrderDetail { OrderDetailId = 62, OrderId = 18, ProductId = 18, Quantity = 1, Price = 20000, Notes = "" },

                // Order 19 — Dewi Lestari Rp90.000 → Ayam Bakar(3x) + Es Jeruk(1x) + Es Teh(1x)
                new OrderDetail { OrderDetailId = 63, OrderId = 19, ProductId = 3,  Quantity = 3, Price = 30000, Notes = "Paha semua" },
                new OrderDetail { OrderDetailId = 64, OrderId = 19, ProductId = 16, Quantity = 1, Price = 8000,  Notes = "" },
                new OrderDetail { OrderDetailId = 65, OrderId = 19, ProductId = 14, Quantity = 1, Price = 5000,  Notes = "" },

                // Order 20 — Arif Budiman Rp74.000 → Mie Goreng(1x) + Nasi Uduk(1x) + Kopi Susu(1x) + Pisang Goreng(1x) + Es Teh(1x)
                new OrderDetail { OrderDetailId = 66, OrderId = 20, ProductId = 2,  Quantity = 1, Price = 22000, Notes = "Ekstra sambal" },
                new OrderDetail { OrderDetailId = 67, OrderId = 20, ProductId = 4,  Quantity = 1, Price = 28000, Notes = "" },
                new OrderDetail { OrderDetailId = 68, OrderId = 20, ProductId = 17, Quantity = 1, Price = 18000, Notes = "" },
                new OrderDetail { OrderDetailId = 69, OrderId = 20, ProductId = 11, Quantity = 1, Price = 13000, Notes = "" },
                new OrderDetail { OrderDetailId = 70, OrderId = 20, ProductId = 14, Quantity = 1, Price = 5000,  Notes = "" },

                // Order 21 — Sari Indah Rp33.000 → Nasi Bakar(1x) + Es Teh(1x)
                new OrderDetail { OrderDetailId = 71, OrderId = 21, ProductId = 7,  Quantity = 1, Price = 26000, Notes = "" },
                new OrderDetail { OrderDetailId = 72, OrderId = 21, ProductId = 14, Quantity = 1, Price = 5000,  Notes = "" },

                // Order 22 — Eko Baskoro Rp59.000 → Ayam Geprek(1x) + Mie Kuah Tongseng(1x) + Jus Alpukat(1x)
                new OrderDetail { OrderDetailId = 73, OrderId = 22, ProductId = 6,  Quantity = 1, Price = 27000, Notes = "Level sedang" },
                new OrderDetail { OrderDetailId = 74, OrderId = 22, ProductId = 8,  Quantity = 1, Price = 24000, Notes = "" },
                new OrderDetail { OrderDetailId = 75, OrderId = 22, ProductId = 15, Quantity = 1, Price = 15000, Notes = "" },

                // Order 23 — Rian Hidayat Rp47.000 → Mie Goreng(1x) + Kentang Goreng(1x) + Kopi Susu(1x) + Es Teh(1x)
                new OrderDetail { OrderDetailId = 76, OrderId = 23, ProductId = 2,  Quantity = 1, Price = 22000, Notes = "Kurangi garam" },
                new OrderDetail { OrderDetailId = 77, OrderId = 23, ProductId = 9,  Quantity = 1, Price = 15000, Notes = "" },
                new OrderDetail { OrderDetailId = 78, OrderId = 23, ProductId = 17, Quantity = 1, Price = 18000, Notes = "" },
                new OrderDetail { OrderDetailId = 79, OrderId = 23, ProductId = 14, Quantity = 1, Price = 5000,  Notes = "" },

                // Order 24 — Claudia Santoso Rp38.000 → Soto Ayam(1x) + Roti Bakar(1x) → Belum Bayar
                new OrderDetail { OrderDetailId = 80, OrderId = 24, ProductId = 5,  Quantity = 1, Price = 22000, Notes = "Tidak mau pedas" },
                new OrderDetail { OrderDetailId = 81, OrderId = 24, ProductId = 13, Quantity = 1, Price = 16000, Notes = "" },

                // Order 25 — Dimas Arya Rp66.000 → Nasi Goreng(1x) + Ayam Bakar(1x) + Matcha(1x) + Tahu Crispy(1x) → Belum Bayar
                new OrderDetail { OrderDetailId = 82, OrderId = 25, ProductId = 1,  Quantity = 1, Price = 25000, Notes = "" },
                new OrderDetail { OrderDetailId = 83, OrderId = 25, ProductId = 3,  Quantity = 1, Price = 30000, Notes = "" },
                new OrderDetail { OrderDetailId = 84, OrderId = 25, ProductId = 18, Quantity = 1, Price = 20000, Notes = "" },
                new OrderDetail { OrderDetailId = 85, OrderId = 25, ProductId = 12, Quantity = 1, Price = 10000, Notes = "" }
            );
        }
    }
}
