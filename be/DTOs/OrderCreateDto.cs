namespace be.DTOs
{
    public class OrderCreateDto
    {
        public string TableNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string? AdditionalNotes { get; set; }
        public List<CartItemDto> CartItems { get; set; } = new();
    }

    public class CartItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string? Notes { get; set; } // Contoh: "mie pedas level 3 tanpa daun bawang"
    }
}