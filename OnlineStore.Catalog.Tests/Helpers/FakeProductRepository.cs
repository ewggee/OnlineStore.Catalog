using OnlineStore.Catalog.Domain.Entities;
using OnlineStore.Catalog.Domain.Repositories;
using OnlineStore.Catalog.Domain.Specifications;
using System.Linq.Expressions;

namespace OnlineStore.Catalog.Tests.Helpers
{
    public class FakeProductRepository : IProductRepository
    {
        public Task<int> AddAsync(Product product, CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        public Task AddRangeAsync(Product[] products, CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Product entity, CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        public Task<List<Product>> GetAllAsync(BaseSpecification<Product> spec, CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        public Task<List<Product>> GetAllAsync(int categoryId, CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        public Task<List<Product>> GetAllAsync(int[] ids, CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        public Task<Product[]> GetAllAsync(CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        public Task<Product[]> GetAllAsync(Expression<Func<Product, bool>> predicate, CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        public Task<Product?> GetAsync(int id, CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetProductsTotalCountAsync(int categoryId, CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsCategoryHasProductsAsync(int categoryId, CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Product entity, CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        public Task UpdateProductsCountAsync(Product[] products, CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        Task IRepository<Product>.AddAsync(Product entity, CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }
    }
}
