using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace be.Migrations
{
    /// <inheritdoc />
    public partial class ExpandFullSeedDataV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ── Step 1: Hapus semua seed data lama (urutan: detail → order → product) ──
            migrationBuilder.Sql(@"DELETE FROM ""OrderDetails"" WHERE ""OrderDetailId"" IN (1,2,3,4,5,6,7,8,9);");
            migrationBuilder.Sql(@"DELETE FROM ""Orders"" WHERE ""OrderId"" IN (1,2,3,4,5);");
            migrationBuilder.Sql(@"DELETE FROM ""Products"" WHERE ""ProductId"" IN (1,2,3,4,5);");

            // ── Step 2: Insert 18 Produk baru dengan kategori yang benar ──
            migrationBuilder.Sql(@"
INSERT INTO ""Products"" (""ProductId"", ""ProductName"", ""Category"", ""Price"", ""Description"", ""ImageUrl"", ""IsAvailable"") VALUES
(1,  'Nasi Goreng Spesial',    'Makanan Berat',   25000, 'Nasi goreng ayam plus telur mata sapi & acar',                     NULL, true),
(2,  'Mie Goreng Jawa',        'Makanan Berat',   22000, 'Mie goreng khas Jawa, pedas manis dengan kecap',                   NULL, true),
(3,  'Ayam Bakar Madu',        'Makanan Berat',   30000, 'Ayam bakar bumbu madu legit, disajikan dengan lalapan',            NULL, true),
(4,  'Nasi Uduk Komplit',      'Makanan Berat',   28000, 'Nasi uduk dengan ayam goreng, tempe, orek tempe & sambal',         NULL, true),
(5,  'Soto Ayam Bening',       'Makanan Berat',   22000, 'Soto ayam kuah bening khas Jawa dengan tauge & soun',              NULL, true),
(6,  'Ayam Geprek Keju',       'Makanan Berat',   27000, 'Ayam geprek crispy dengan lelehan keju mozzarella panas',          NULL, true),
(7,  'Nasi Bakar Isi Ayam',    'Makanan Berat',   26000, 'Nasi bakar dibungkus daun pisang isi ayam suwir pedas',            NULL, true),
(8,  'Mie Kuah Tongseng',      'Makanan Berat',   24000, 'Mie kuah dengan daging tongseng kambing & sayuran segar',          NULL, true),
(9,  'Kentang Goreng Crispy',  'Makanan Ringan',  15000, 'Kentang goreng renyah seasoning BBQ, disajikan dengan saus',       NULL, true),
(10, 'Cireng Bumbu Rujak',     'Makanan Ringan',  12000, 'Cireng aci goreng dengan cocolan bumbu rujak pedas manis',         NULL, true),
(11, 'Pisang Goreng Coklat',   'Makanan Ringan',  13000, 'Pisang goreng crispy topping coklat & keju leleh',                 NULL, true),
(12, 'Tahu Crispy Sambal Ijo', 'Makanan Ringan',  10000, 'Tahu goreng crispy disajikan dengan sambal hijau segar',           NULL, true),
(13, 'Roti Bakar Nutella',     'Makanan Ringan',  16000, 'Roti bakar tebal dengan selai Nutella & topping susu kental',      NULL, true),
(14, 'Es Teh Manis',           'Minuman',          5000, 'Es teh manis segar, diseduh dari teh pilihan',                    NULL, true),
(15, 'Jus Alpukat Shaked',     'Minuman',         15000, 'Jus alpukat creamy dengan kerokan es krim vanilla',               NULL, true),
(16, 'Es Jeruk Peras',         'Minuman',          8000, 'Jeruk lokal segar diperas langsung, manis asam menyegarkan',      NULL, true),
(17, 'Kopi Susu Gula Aren',    'Minuman',         18000, 'Kopi robusta blend susu segar dengan gula aren pilihan',          NULL, true),
(18, 'Matcha Latte',           'Minuman',         20000, 'Matcha premium Jepang dipadukan susu oat, lembut & gurih',        NULL, true);
");


            // ── Step 3: Insert 25 Orders (Mei – Juli 2026) ──
            migrationBuilder.Sql(@"
INSERT INTO ""Orders"" (""OrderId"", ""TableNumber"", ""CustomerName"", ""OrderDate"", ""TotalAmount"", ""Status"", ""AdditionalNotes"") VALUES
(1,  'T-02', 'Budi Santoso',    '2026-05-03 11:30:00Z', 50000,  'Lunas',       'Pedas semua ya'),
(2,  'T-05', 'Siti Rahma',      '2026-05-08 19:15:00Z', 65000,  'Lunas',       'Minuman belakangan'),
(3,  'T-01', 'Andi Wijaya',     '2026-05-12 12:00:00Z', 43000,  'Lunas',       ''),
(4,  'T-07', 'Rina Oktavia',    '2026-05-15 13:45:00Z', 78000,  'Lunas',       'Tidak pedas'),
(5,  'T-03', 'Hendra Gunawan',  '2026-05-18 18:00:00Z', 56000,  'Lunas',       'Ekstra sambal'),
(6,  'T-10', 'Mega Putri',      '2026-05-22 20:30:00Z', 48000,  'Lunas',       ''),
(7,  'T-04', 'Fajar Nugroho',   '2026-05-27 10:15:00Z', 35000,  'Lunas',       'Kurangi gula'),
(8,  'T-08', 'Laila Sari',      '2026-05-30 14:00:00Z', 62000,  'Lunas',       'Pisah tagihan'),
(9,  'T-06', 'Doni Prasetyo',   '2026-06-02 12:00:00Z', 57000,  'Lunas',       'Ayam paha semua'),
(10, 'T-02', 'Wulandari',       '2026-06-07 17:30:00Z', 46000,  'Lunas',       ''),
(11, 'T-09', 'Rizky Aditya',    '2026-06-11 19:00:00Z', 85000,  'Lunas',       'Tambah nasi'),
(12, 'T-01', 'Putri Andini',    '2026-06-14 13:00:00Z', 38000,  'Lunas',       ''),
(13, 'T-05', 'Bambang Irawan',  '2026-06-18 11:45:00Z', 70000,  'Lunas',       'Tidak pedas sama sekali'),
(14, 'T-03', 'Nadia Fitriani',  '2026-06-22 20:00:00Z', 55000,  'Lunas',       'Ekstra keju'),
(15, 'T-07', 'Surya Dharma',    '2026-06-25 12:30:00Z', 49000,  'Lunas',       ''),
(16, 'T-10', 'Intan Permata',   '2026-06-28 14:15:00Z', 41000,  'Lunas',       'Tanpa bawang'),
(17, 'T-04', 'Yoga Pratama',    '2026-06-30 21:00:00Z', 63000,  'Lunas',       'Manis sekali'),
(18, 'T-06', 'Raka Santika',    '2026-07-01 10:00:00Z', 52000,  'Lunas',       ''),
(19, 'T-02', 'Dewi Lestari',    '2026-07-03 12:45:00Z', 90000,  'Lunas',       'Ayam paha semua'),
(20, 'T-08', 'Arif Budiman',    '2026-07-05 19:00:00Z', 74000,  'Lunas',       'Ekstra sambal'),
(21, 'T-01', 'Sari Indah',      '2026-07-07 13:30:00Z', 33000,  'Lunas',       ''),
(22, 'T-09', 'Eko Baskoro',     '2026-07-08 18:00:00Z', 59000,  'Lunas',       'Level pedas sedang'),
(23, 'T-05', 'Rian Hidayat',    '2026-07-09 08:45:00Z', 47000,  'Lunas',       'Kurangi garam'),
(24, 'T-03', 'Claudia Santoso', '2026-07-09 14:00:00Z', 38000,  'Belum Bayar', 'Tidak mau pedas'),
(25, 'T-07', 'Dimas Arya',      '2026-07-09 20:15:00Z', 66000,  'Belum Bayar', 'Minta tisu banyak');
");


            // ── Step 4: Insert 85 OrderDetails ──
            migrationBuilder.Sql(@"
INSERT INTO ""OrderDetails"" (""OrderDetailId"", ""OrderId"", ""ProductId"", ""Quantity"", ""Price"", ""Notes"") VALUES
-- Order 1: Budi Santoso → Nasi Goreng(2x) + Es Teh(2x)
(1,  1,  1,  2, 25000, 'Pedas'),
(2,  1,  14, 2, 5000,  ''),
-- Order 2: Siti Rahma → Mie Goreng(1x) + Ayam Bakar(1x) + Kopi Susu(1x) + Es Teh(1x)
(3,  2,  2,  1, 22000, 'Sedang'),
(4,  2,  3,  1, 30000, ''),
(5,  2,  17, 1, 18000, ''),
(6,  2,  14, 1, 5000,  'Belakangan'),
-- Order 3: Andi Wijaya → Soto Ayam(1x) + Kentang Goreng(1x) + Es Jeruk(1x)
(7,  3,  5,  1, 22000, ''),
(8,  3,  9,  1, 15000, ''),
(9,  3,  16, 1, 8000,  'Sedikit gula'),
-- Order 4: Rina Oktavia → Nasi Uduk(2x) + Matcha(1x) + Cireng(1x) + Es Teh(1x)
(10, 4,  4,  2, 28000, 'Tidak pedas'),
(11, 4,  18, 1, 20000, ''),
(12, 4,  10, 1, 12000, ''),
(13, 4,  14, 1, 5000,  ''),
-- Order 5: Hendra Gunawan → Ayam Geprek(1x) + Kentang Goreng(1x) + Kopi Susu(1x) + Es Teh(1x)
(14, 5,  6,  1, 27000, 'Level 3'),
(15, 5,  9,  1, 15000, 'Ekstra saus'),
(16, 5,  17, 1, 18000, ''),
(17, 5,  14, 1, 5000,  ''),
-- Order 6: Mega Putri → Mie Kuah Tongseng(1x) + Matcha(1x) + Pisang Goreng(1x)
(18, 6,  8,  1, 24000, ''),
(19, 6,  18, 1, 20000, ''),
(20, 6,  11, 1, 13000, 'Ekstra coklat'),
-- Order 7: Fajar Nugroho → Nasi Bakar(1x) + Es Teh(1x) + Tahu Crispy(1x)
(21, 7,  7,  1, 26000, ''),
(22, 7,  14, 1, 5000,  'Kurang gula'),
(23, 7,  12, 1, 10000, ''),
-- Order 8: Laila Sari → Nasi Goreng(1x) + Ayam Bakar(1x) + Roti Bakar(1x) + Es Jeruk(1x)
(24, 8,  1,  1, 25000, 'Pedas sedang'),
(25, 8,  3,  1, 30000, ''),
(26, 8,  13, 1, 16000, ''),
(27, 8,  16, 1, 8000,  ''),
-- Order 9: Doni Prasetyo → Ayam Bakar(1x) + Soto Ayam(1x) + Kopi Susu(1x) + Es Teh(1x)
(28, 9,  3,  1, 30000, 'Paha'),
(29, 9,  5,  1, 22000, ''),
(30, 9,  17, 1, 18000, ''),
(31, 9,  14, 1, 5000,  ''),
-- Order 10: Wulandari → Nasi Uduk(1x) + Jus Alpukat(1x) + Cireng(1x)
(32, 10, 4,  1, 28000, ''),
(33, 10, 15, 1, 15000, ''),
(34, 10, 10, 1, 12000, ''),
-- Order 11: Rizky Aditya → Ayam Geprek(2x) + Kentang Goreng(2x) + Es Teh(2x) + Matcha(1x)
(35, 11, 6,  2, 27000, 'Level 5'),
(36, 11, 9,  2, 15000, ''),
(37, 11, 14, 2, 5000,  ''),
(38, 11, 18, 1, 20000, ''),
-- Order 12: Putri Andini → Mie Goreng(1x) + Tahu Crispy(1x) + Es Jeruk(1x)
(39, 12, 2,  1, 22000, 'Tidak terlalu manis'),
(40, 12, 12, 1, 10000, ''),
(41, 12, 16, 1, 8000,  ''),
-- Order 13: Bambang Irawan → Nasi Goreng(1x) + Ayam Bakar(1x) + Kopi Susu(1x) + Roti Bakar(1x) + Es Teh(1x)
(42, 13, 1,  1, 25000, 'Tidak pedas'),
(43, 13, 3,  1, 30000, 'Tidak pedas'),
(44, 13, 17, 1, 18000, ''),
(45, 13, 13, 1, 16000, ''),
(46, 13, 14, 1, 5000,  ''),
-- Order 14: Nadia Fitriani → Ayam Geprek(1x) + Matcha(1x) + Pisang Goreng(1x)
(47, 14, 6,  1, 27000, 'Ekstra keju'),
(48, 14, 18, 1, 20000, ''),
(49, 14, 11, 1, 13000, 'Ekstra coklat'),
-- Order 15: Surya Dharma → Soto Ayam(1x) + Nasi Bakar(1x) + Kopi Susu(1x)
(50, 15, 5,  1, 22000, ''),
(51, 15, 7,  1, 26000, ''),
(52, 15, 17, 1, 18000, ''),
-- Order 16: Intan Permata → Mie Kuah Tongseng(1x) + Es Teh(1x) + Cireng(1x)
(53, 16, 8,  1, 24000, 'Tanpa bawang'),
(54, 16, 14, 1, 5000,  ''),
(55, 16, 10, 1, 12000, ''),
-- Order 17: Yoga Pratama → Nasi Uduk(1x) + Ayam Geprek(1x) + Jus Alpukat(1x) + Tahu Crispy(1x)
(56, 17, 4,  1, 28000, ''),
(57, 17, 6,  1, 27000, 'Sangat manis'),
(58, 17, 15, 1, 15000, ''),
(59, 17, 12, 1, 10000, ''),
-- Order 18: Raka Santika → Nasi Goreng(1x) + Soto Ayam(1x) + Matcha(1x)
(60, 18, 1,  1, 25000, ''),
(61, 18, 5,  1, 22000, ''),
(62, 18, 18, 1, 20000, ''),
-- Order 19: Dewi Lestari → Ayam Bakar(3x) + Es Jeruk(1x) + Es Teh(1x)
(63, 19, 3,  3, 30000, 'Paha semua'),
(64, 19, 16, 1, 8000,  ''),
(65, 19, 14, 1, 5000,  ''),
-- Order 20: Arif Budiman → Mie Goreng(1x) + Nasi Uduk(1x) + Kopi Susu(1x) + Pisang Goreng(1x) + Es Teh(1x)
(66, 20, 2,  1, 22000, 'Ekstra sambal'),
(67, 20, 4,  1, 28000, ''),
(68, 20, 17, 1, 18000, ''),
(69, 20, 11, 1, 13000, ''),
(70, 20, 14, 1, 5000,  ''),
-- Order 21: Sari Indah → Nasi Bakar(1x) + Es Teh(1x)
(71, 21, 7,  1, 26000, ''),
(72, 21, 14, 1, 5000,  ''),
-- Order 22: Eko Baskoro → Ayam Geprek(1x) + Mie Kuah Tongseng(1x) + Jus Alpukat(1x)
(73, 22, 6,  1, 27000, 'Level sedang'),
(74, 22, 8,  1, 24000, ''),
(75, 22, 15, 1, 15000, ''),
-- Order 23: Rian Hidayat → Mie Goreng(1x) + Kentang Goreng(1x) + Kopi Susu(1x) + Es Teh(1x)
(76, 23, 2,  1, 22000, 'Kurangi garam'),
(77, 23, 9,  1, 15000, ''),
(78, 23, 17, 1, 18000, ''),
(79, 23, 14, 1, 5000,  ''),
-- Order 24: Claudia Santoso → Soto Ayam(1x) + Roti Bakar(1x) [Belum Bayar]
(80, 24, 5,  1, 22000, 'Tidak mau pedas'),
(81, 24, 13, 1, 16000, ''),
-- Order 25: Dimas Arya → Nasi Goreng(1x) + Ayam Bakar(1x) + Matcha(1x) + Tahu Crispy(1x) [Belum Bayar]
(82, 25, 1,  1, 25000, ''),
(83, 25, 3,  1, 30000, ''),
(84, 25, 18, 1, 20000, ''),
(85, 25, 12, 1, 10000, '');
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Hapus semua data baru
            migrationBuilder.Sql(@"DELETE FROM ""OrderDetails"" WHERE ""OrderDetailId"" BETWEEN 1 AND 85;");
            migrationBuilder.Sql(@"DELETE FROM ""Orders"" WHERE ""OrderId"" BETWEEN 1 AND 25;");
            migrationBuilder.Sql(@"DELETE FROM ""Products"" WHERE ""ProductId"" BETWEEN 1 AND 18;");

            // Kembalikan seed data awal (5 produk lama)
            migrationBuilder.Sql(@"
INSERT INTO ""Products"" (""ProductId"", ""ProductName"", ""Category"", ""Price"", ""Description"", ""ImageUrl"", ""IsAvailable"") VALUES
(1, 'Nasi Goreng Spesial',  'Makanan', 25000, 'Nasi goreng ayam plus telur', NULL, true),
(2, 'Mie Goreng Jawa',      'Makanan', 22000, 'Mie goreng khas Jawa pedas manis', NULL, true),
(3, 'Ayam Bakar Madu',      'Makanan', 30000, 'Ayam bakar bumbu madu legit', NULL, true),
(4, 'Es Teh Manis',         'Minuman',  5000, 'Es teh manis segar', NULL, true),
(5, 'Jus Alpukat Shaked',   'Minuman', 15000, 'Jus alpukat dengan kerokan es krim', NULL, true);
");
        }
    }
}
