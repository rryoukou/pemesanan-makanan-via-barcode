using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace be.Migrations
{
    /// <inheritdoc />
    public partial class AddProductImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Update ImageUrl untuk semua 18 produk menggunakan Unsplash source (CDN stabil, no API key)
            migrationBuilder.Sql(@"
UPDATE ""Products"" SET ""ImageUrl"" = 'https://images.unsplash.com/photo-1512058564366-18510be2db19?w=600&q=80' WHERE ""ProductId"" = 1;
UPDATE ""Products"" SET ""ImageUrl"" = 'https://images.unsplash.com/photo-1585032226651-759b368d7246?w=600&q=80' WHERE ""ProductId"" = 2;
UPDATE ""Products"" SET ""ImageUrl"" = 'https://images.unsplash.com/photo-1529193591184-b1d58069ecdd?w=600&q=80' WHERE ""ProductId"" = 3;
UPDATE ""Products"" SET ""ImageUrl"" = 'https://images.unsplash.com/photo-1567620905732-2d1ec7ab7445?w=600&q=80' WHERE ""ProductId"" = 4;
UPDATE ""Products"" SET ""ImageUrl"" = 'https://images.unsplash.com/photo-1555126634-323283e090fa?w=600&q=80' WHERE ""ProductId"" = 5;
UPDATE ""Products"" SET ""ImageUrl"" = 'https://images.unsplash.com/photo-1626082927389-6cd097cdc6ec?w=600&q=80' WHERE ""ProductId"" = 6;
UPDATE ""Products"" SET ""ImageUrl"" = 'https://images.unsplash.com/photo-1598515214211-89d3c73ae83b?w=600&q=80' WHERE ""ProductId"" = 7;
UPDATE ""Products"" SET ""ImageUrl"" = 'https://images.unsplash.com/photo-1569050467447-ce54b3bbc37d?w=600&q=80' WHERE ""ProductId"" = 8;
UPDATE ""Products"" SET ""ImageUrl"" = 'https://images.unsplash.com/photo-1630384060421-cb20d0e0649d?w=600&q=80' WHERE ""ProductId"" = 9;
UPDATE ""Products"" SET ""ImageUrl"" = 'https://images.unsplash.com/photo-1606914707244-e398ddb0e370?w=600&q=80' WHERE ""ProductId"" = 10;
UPDATE ""Products"" SET ""ImageUrl"" = 'https://images.unsplash.com/photo-1528975604071-b4dc52a2d18c?w=600&q=80' WHERE ""ProductId"" = 11;
UPDATE ""Products"" SET ""ImageUrl"" = 'https://images.unsplash.com/photo-1546069901-ba9599a7e63c?w=600&q=80' WHERE ""ProductId"" = 12;
UPDATE ""Products"" SET ""ImageUrl"" = 'https://images.unsplash.com/photo-1484723091739-30a097e8f929?w=600&q=80' WHERE ""ProductId"" = 13;
UPDATE ""Products"" SET ""ImageUrl"" = 'https://images.unsplash.com/photo-1556679343-c7306c1976bc?w=600&q=80' WHERE ""ProductId"" = 14;
UPDATE ""Products"" SET ""ImageUrl"" = 'https://images.unsplash.com/photo-1551023916-61b31b05de9b?w=600&q=80' WHERE ""ProductId"" = 15;
UPDATE ""Products"" SET ""ImageUrl"" = 'https://images.unsplash.com/photo-1621506289937-a8e4df240d0b?w=600&q=80' WHERE ""ProductId"" = 16;
UPDATE ""Products"" SET ""ImageUrl"" = 'https://images.unsplash.com/photo-1461023058943-07fcbe16d735?w=600&q=80' WHERE ""ProductId"" = 17;
UPDATE ""Products"" SET ""ImageUrl"" = 'https://images.unsplash.com/photo-1536256263959-770b48d82b0a?w=600&q=80' WHERE ""ProductId"" = 18;
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"UPDATE ""Products"" SET ""ImageUrl"" = NULL WHERE ""ProductId"" BETWEEN 1 AND 18;");
        }
    }
}
