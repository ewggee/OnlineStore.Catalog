using OnlineStore.Catalog.Contracts.Enums;
using OnlineStore.Catalog.Contracts.Requests;
using OnlineStore.Catalog.Domain.Entities;
using OnlineStore.Catalog.Domain.Specifications;
using System.Linq.Expressions;

namespace OnlineStore.Catalog.Infrastructure.Specifications
{
    public class ProductSpecification : BaseSpecification<Product>
    {
        private readonly GetProductsRequest _request;
        private readonly int _pageSize;

        public ProductSpecification(GetProductsRequest request, int pageSize)
        {
            _request = request;
            _pageSize = pageSize;
        }

        public override Expression<Func<Product, bool>> Criteria =>
            product =>
            product.CategoryId == _request.CategoryId &&
            product.IsDeleted == false;

        public override Func<IQueryable<Product>, IOrderedQueryable<Product>> Ordering =>
            query =>
            _request.Sort switch
            {
                ProductsSortEnum.PriceDesc => query.OrderByDescending(p => p.Price),
                ProductsSortEnum.PriceAsc => query.OrderBy(p => p.Price),
                ProductsSortEnum.NoveltyDesc => query.OrderByDescending(p => p.CreatedAt),
                ProductsSortEnum.NoveltyAsc => query.OrderBy(p => p.CreatedAt),
                ProductsSortEnum.Default => query.OrderBy(p => p.Id),
                _ => throw new ArgumentException("Incorrect product sorting method.")
            };

        public override (int Number, int Size)? Pagination => (Number: _request.Page, Size: _pageSize);
    }
}
