using OnlineStore.Catalog.Application.Abstractions;
using OnlineStore.Catalog.Contracts.Requests;
using OnlineStore.Catalog.Domain.Entities;
using OnlineStore.Catalog.Domain.Specifications;
using OnlineStore.Catalog.Infrastructure.Specifications;

namespace OnlineStore.Catalog.Infrastructure.Data
{
    public class SpecificationFactory : ISpecificationFactory
    {
        public BaseSpecification<Product> CreateProductSpecification(GetProductsRequest request, int pageSize)
        {
            return new ProductSpecification(request, pageSize);
        }
    }
}
