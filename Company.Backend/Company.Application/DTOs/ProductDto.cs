namespace Company.Application.DTOs
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Inventory { get; set; }
        public decimal Price { get; set; }
    }
}
