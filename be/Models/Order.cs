namespace be.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string TableNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Belum Bayar"; // Belum Bayar, Lunas
        public string? AdditionalNotes { get; set; }

        // Relasi ke detail item
        public List<OrderDetail> OrderDetails { get; set; } = new();
    }
}