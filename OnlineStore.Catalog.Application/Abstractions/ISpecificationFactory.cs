using OnlineStore.Catalog.Contracts.Requests;
using OnlineStore.Catalog.Domain.Entities;
using OnlineStore.Catalog.Domain.Specifications;

namespace OnlineStore.Catalog.Application.Abstractions
{
    public interface ISpecificationFactory
    {
        BaseSpecification<Product> CreateProductSpecification(GetProductsRequest request, int pageSize);
    }
}
