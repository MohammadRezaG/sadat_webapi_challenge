using Sadas_test.Models.Domain;

namespace Sadas_test.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task SaveProductAsync(Product product);
        Task<IEnumerable<Product>> GetAllProductsAsync();
    }
}
