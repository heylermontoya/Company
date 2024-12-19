namespace Company.Application.DTOs
{
    public class TransactionDto
    {
        public int TransactionId { get; set; }
        public ProductDto Product { get; set; } = new ProductDto();
        public UserDto User { get; set; } = new UserDto();
        public int Quantity { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
