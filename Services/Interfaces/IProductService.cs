using Sadas_test.Models.Domain;
using Sadas_test.Repositories.Interfaces;

namespace Sadas_test.Services.Interfaces
{
    public interface IProductService
    {
        Task SaveProductAsync(Product product);
        Task<IEnumerable<Product>> GetAllProductsAsync();
    }

}
