namespace Sadas_test.Models.Domain
{
    public class Product
    {
        public int Id { get; set; }                   // Primary Key
        public required string ProductName { get; set; }       // Input
        public required decimal Price { get; set; }            // Output
        public required string Source { get; set; }            // Where it was fetched from
        public DateTime RetrievedAt { get; set; } = DateTime.UtcNow;
    }
}
