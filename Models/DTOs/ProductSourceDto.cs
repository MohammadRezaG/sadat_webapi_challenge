namespace Sadas_test.Models.DTOs
{
    public class ProductSourceDto
    {
        public required string SourceName { get; set; }
        public required string ApiUrl { get; set; }
        public required int NumParameters { get; set; } = 1;
        public required string ParametersMagicStr { get; set; } = "{{#}}";
    }
}
