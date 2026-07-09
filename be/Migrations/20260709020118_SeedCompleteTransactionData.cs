using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace be.Migrations
{
    /// <inheritdoc />
    public partial class SeedCompleteTransactionData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "OrderId", "AdditionalNotes", "CustomerName", "OrderDate", "Status", "TableNumber", "TotalAmount" },
                values: new object[,]
                {
                    { 1, "Pedas semua ya", "Budi Santoso", new DateTime(2026, 6, 22, 12, 30, 0, 0, DateTimeKind.Utc), "Lunas", "T-02", 55000m },
                    { 2, "Es teh belakangan", "Siti Rahma", new DateTime(2026, 6, 25, 19, 15, 0, 0, DateTimeKind.Utc), "Lunas", "T-05", 49000m },
                    { 3, "", "Andi Wijaya", new DateTime(2026, 7, 1, 10, 0, 0, 0, DateTimeKind.Utc), "Lunas", "T-01", 30000m },
                    { 4, "Ayam paha semua", "Dewi Lestari", new DateTime(2026, 7, 8, 14, 20, 0, 0, DateTimeKind.Utc), "Lunas", "T-09", 90000m },
                    { 5, "Kurangi garam", "Rian Hidayat", new DateTime(2026, 7, 9, 8, 45, 0, 0, DateTimeKind.Utc), "Pending", "T-03", 27000m }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "Category", "Description", "ImageUrl", "IsAvailable", "Price", "ProductName" },
                values: new object[,]
                {
                    { 1, "Makanan", "Nasi goreng ayam plus telur", null, true, 25000m, "Nasi Goreng Spesial" },
                    { 2, "Makanan", "Mie goreng khas Jawa pedas manis", null, true, 22000m, "Mie Goreng Jawa" },
                    { 3, "Makanan", "Ayam bakar bumbu madu legit", null, true, 30000m, "Ayam Bakar Madu" },
                    { 4, "Minuman", "Es teh manis segar", null, true, 5000m, "Es Teh Manis" },
                    { 5, "Minuman", "Jus alpukat dengan kerokan es krim", null, true, 15000m, "Jus Alpukat Shaked" }
                });

            migrationBuilder.InsertData(
                table: "OrderDetails",
                columns: new[] { "OrderDetailId", "Notes", "OrderId", "Price", "ProductId", "Quantity" },
                values: new object[,]
                {
                    { 1, "Pedas", 1, 25000m, 1, 2 },
                    { 2, "", 1, 5000m, 4, 1 },
                    { 3, "Sedang", 2, 22000m, 2, 1 },
                    { 4, "", 2, 30000m, 3, 1 },
                    { 5, "", 2, 5000m, 4, 1 },
                    { 6, "", 3, 30000m, 3, 1 },
                    { 7, "", 4, 30000m, 3, 3 },
                    { 8, "", 5, 22000m, 2, 1 },
                    { 9, "", 5, 5000m, 4, 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OrderDetails",
                keyColumn: "OrderDetailId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "OrderDetails",
                keyColumn: "OrderDetailId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "OrderDetails",
                keyColumn: "OrderDetailId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "OrderDetails",
                keyColumn: "OrderDetailId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "OrderDetails",
                keyColumn: "OrderDetailId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "OrderDetails",
                keyColumn: "OrderDetailId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "OrderDetails",
                keyColumn: "OrderDetailId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "OrderDetails",
                keyColumn: "OrderDetailId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "OrderDetails",
                keyColumn: "OrderDetailId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 4);
        }
    }
}
