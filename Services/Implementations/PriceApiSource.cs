using Sadas_test.Services.Interfaces;

namespace Sadas_test.Services.Implementations
{
    public class PriceApiSource() : IApiSource
    {
        public required string Name { get; set; }
        public required string ApiUrl {  get; set; }
        public required int NumParameters { get; set; }
        public required string ParametersMagicStr { get; set; }

    }
}
