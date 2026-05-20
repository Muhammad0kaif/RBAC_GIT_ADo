namespace MVCview.Models
{
    public class OrderDto
    {
        public Guid Id { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public Guid UserId { get; set; }
    }
}
