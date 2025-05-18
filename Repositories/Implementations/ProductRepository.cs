using System.Data;
using Dapper;
using Sadas_test.Data;
using Sadas_test.Models.Domain;
using Sadas_test.Repositories.Interfaces;

namespace Sadas_test.Repositories.Implementations
{
    public class ProductRepository : IProductRepository
    {
        private readonly DbContext _dbContext;

        public ProductRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SaveProductAsync(Product product)
        {
            const string sql = @"
                INSERT INTO Products (ProductName, Price, Source, RetrievedAt)
                VALUES (@ProductName, @Price, @Source, @RetrievedAt);";

            using IDbConnection connection = _dbContext.CreateConnection();
            await connection.ExecuteAsync(sql, product);
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            const string sql = "SELECT * FROM Products ORDER BY RetrievedAt DESC";

            using IDbConnection connection = _dbContext.CreateConnection();
            return await connection.QueryAsync<Product>(sql);
        }
    }
}
