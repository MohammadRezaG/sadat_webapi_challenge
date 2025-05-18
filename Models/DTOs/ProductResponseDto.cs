namespace Sadas_test.Models.DTOs
{
    public class ProductResponseDto
    {
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Source { get; set; } = string.Empty;
        public DateTime RetrievedAt { get; set; }
    }
}
