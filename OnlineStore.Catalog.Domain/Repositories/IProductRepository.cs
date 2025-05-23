﻿using OnlineStore.Catalog.Domain.Entities;
using OnlineStore.Catalog.Domain.Specifications;

namespace OnlineStore.Catalog.Domain.Repositories;

public interface IProductRepository : IRepository<Product>
{
    /// <summary>
    /// Возвращает список товаров по спецификации <see cref="ISpecification{T}" />.
    /// </summary>
    Task<List<Product>> GetAllAsync(BaseSpecification<Product> spec, CancellationToken cancellation);

    /// <summary>
    /// Возвращает список товаров по ID категории.
    /// </summary>
    Task<List<Product>> GetAllAsync(int categoryId, CancellationToken cancellation);

    /// <summary>
    /// Возвращает список товаров по их ID.
    /// </summary>
    /// <param name="ids">IDs товаров.</param>
    Task<List<Product>> GetAllAsync(int[] ids, CancellationToken cancellation);

    /// <summary>
    /// Возвращает общее количество всех товаров в категории.
    /// </summary>
    /// <param name="categoryId">ID категории.</param>
    Task<int> GetProductsTotalCountAsync(int categoryId, CancellationToken cancellation);

    /// <summary>
    /// Добавляет товар в БД и возвращает сгенерированный ID записи.
    /// </summary>
    new Task<int> AddAsync(Product product, CancellationToken cancellation);

    /// <summary>
    /// Обновляет значение StockQuantity у всех товаров в массиве.
    /// </summary>
    Task UpdateProductsCountAsync(Product[] products, CancellationToken cancellation);

    /// <summary>
    /// Возвращает <b>true</b>, если в категории с ID <paramref name="categoryId"/> есть товары, иначе <b>false</b>.
    /// </summary>
    /// <param name="categoryId">ID категории.</param>
    Task<bool> IsCategoryHasProductsAsync(int categoryId, CancellationToken cancellation);
}
