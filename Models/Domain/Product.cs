namespace Sadas_test.Models.Domain
{
    public class Product
    {
        public int Id { get; set; } 
        public required string ProductName { get; set; } 
        public required decimal Price { get; set; } 
        public required string Source { get; set; } 
        public DateTime RetrievedAt { get; set; } = DateTime.UtcNow;
    }
}
