using System.Linq.Expressions;

namespace OnlineStore.Catalog.Domain.Specifications
{
    /// <summary>
    /// Базовый класс для спецификаций.
    /// </summary>
    public abstract class BaseSpecification<T>
    {
        /// <summary>
        /// Условия выборки.
        /// </summary>
        public virtual Expression<Func<T, bool>>? Criteria { get; }
        /// <summary>
        /// Метод сортировки.
        /// </summary>
        public virtual Func<IQueryable<T>, IOrderedQueryable<T>>? Ordering { get; }
        /// <summary>
        /// Связаные данные.
        /// </summary>
        public virtual Func<IQueryable<T>, IQueryable<T>>? Includes { get; }
        /// <summary>
        /// Параметры пагинации.<br>
        /// <b>Number</b> — номер страницы.
        /// <b>Size</b> — размер страницы (количество элементов на странице).</br>
        /// </summary>
        public virtual (int Number, int Size)? Pagination { get; }

        /// <summary>
        /// Применяет все параметры спецификации для запроса <paramref name="query"/>.
        /// </summary>
        public IQueryable<T> ApplySpecification(IQueryable<T> query)
        {
            if (Criteria != null)
            {
                query = query.Where(Criteria);
            }

            if (Ordering != null)
            {
                query = Ordering(query);
            }

            if (Includes != null)
            {
                query = Includes(query);
            }

            if (Pagination.HasValue)
            {
                var pageNumber = Pagination.Value.Number;
                var pageSize = Pagination.Value.Size;

                query = query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize);
            }

            return query;
        }
    }
}
