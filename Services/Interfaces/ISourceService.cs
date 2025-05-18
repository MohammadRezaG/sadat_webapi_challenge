using Sadas_test.Models.DTOs;
using System.Runtime.CompilerServices;

namespace Sadas_test.Services.Interfaces
{
  

    public interface IPriceSourcesService
    {
        Task<IEnumerable<ProductSourceDto>> GetAllSourcesAsync();

        Task<int> CountSourcesAsync();

        Task<(string SourceName, decimal Price)> CallAllSources(string ProductName);
    }

}
