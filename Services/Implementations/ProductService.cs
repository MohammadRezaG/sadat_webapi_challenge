using Sadas_test.Models.Domain;
using Sadas_test.Repositories.Implementations;
using Sadas_test.Repositories.Interfaces;
using Sadas_test.Services.Interfaces;
using System;


namespace Sadas_test.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;

        public ProductService(IProductRepository repo)
        {
            _repo = repo;
        }

        public Task SaveProductAsync(Product product) => _repo.SaveProductAsync(product);
        public Task<IEnumerable<Product>> GetAllProductsAsync() => _repo.GetAllProductsAsync();
    }
} 



