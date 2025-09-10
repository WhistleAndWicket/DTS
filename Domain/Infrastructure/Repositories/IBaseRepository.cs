using Domain.Entities.Base;
using Domain.Infrastructure.Specifications;

namespace Domain.Infrastructure.Repositories
{
    /// <summary>
    /// Defines the database actions.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBaseRepository<T> where T : Entity
    {
        /// <summary>
        /// Adds an <typeparamref name="T"/>.
        /// </summary>
        /// <param name="entity">The <typeparamref name="T"/> to add.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to use.</param>
        /// <returns>The added <typeparamref name="T"/>.</returns>
        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get all the <typeparamref name="T"/>.
        /// </summary>
        /// <param name="specification">The specification for querying the <typeparamref name="T"/>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to use.</param>
        /// <returns>A list of <typeparamref name="T"/>.</returns>
        Task<List<T>> GetAllAsync(ISpecification<T> specification, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the list of all <typeparamref name="T"/> using the <paramref name="page"/> and <paramref name="pageSize"/>
        /// </summary>
        /// <param name="page">The current page number.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <param name="specification">Optional. The specification for querying the <typeparamref name="T"/>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to use.</param>
        /// <returns>A tuple containing a list of <typeparamref name="T"/> and total count.</returns>
        Task<(List<T> Items, int TotalCount)> GetPageResultsAsync(
            int page, int pageSize, ISpecification<T>? specification = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get the <typeparamref name="T"/> by its <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The id of the <typeparamref name="T"/>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to use.</param>
        /// <returns>The <typeparamref name="T"/>.</returns>
        Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    }
}
