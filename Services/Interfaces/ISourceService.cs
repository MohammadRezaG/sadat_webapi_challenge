using System.Runtime.CompilerServices;

namespace Sadas_test.Services.Interfaces
{
    public interface IApiSource
    {
        string Name { get; set; }
        string ApiUrl { get; set; }
        int NumParameters { get; set; }
        string ParametersMagicStr { get; set; } 
 
    }

    public interface IPriceSourcesService
    {
        Task<IEnumerable<IApiSource>> GetAllSourcesAsync();

        Task<int> CountSourcesAsync();

        Task<(string SourceName, decimal Price)> CallAllSources(string ProductName);
    }

}
