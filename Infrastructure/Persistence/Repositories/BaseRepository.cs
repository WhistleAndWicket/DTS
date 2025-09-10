using Domain.Entities.Base;
using Domain.Infrastructure.Repositories;
using Domain.Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Generic base repository for managing entities with EF Core and specification support.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <param name="dbContext">The EF Core database context.</param>
    public class BaseRepository<T>(DtsDbContext dbContext) : IBaseRepository<T>
    where T : Entity
    {
        protected readonly DtsDbContext dbContext = dbContext;

        #region Specification Helpers

        /// <summary>
        /// Applies AsNoTracking behavior based on the specification.
        /// </summary>
        /// <param name="specification">The specification that defines tracking behavior.</param>
        /// <param name="query">The base <see cref="IQueryable{T}"/> to which the specification is applied.</param>
        /// <returns>An <see cref="IQueryable{T}"/> with AsNoTracking applied if specified.</returns>
        protected static IQueryable<T> ApplyAsNoTracking(
            ISpecification<T> specification,
            IQueryable<T> query
        )
        {
            query = specification.ApplyAsNoTracking ? query.AsNoTracking() : query;
            return query;
        }

        /// <summary>
        /// Applies split query behavior based on the specification.
        /// </summary>
        /// <param name="specification">The specification that defines query-splitting options for the entity query.</param>
        /// <param name="query">The base <see cref="IQueryable{T}"/> to which the specification is applied.</param>
        /// <returns>An <see cref="IQueryable{T}"/> with split query applied if specified.</returns>
        protected static IQueryable<T> ApplyAsSplitQuery(ISpecification<T> specification, IQueryable<T> query)
        {
            if (specification.UseSplitQuery)
            {
                query = query.AsSplitQuery();
            }

            return query;
        }

        /// <summary>
        /// Applies filter predicates defined in the specification.
        /// </summary>
        /// <param name="specification">The specification that defines filtering.</param>
        /// <param name="query">The base <see cref="IQueryable{T}"/> to which the specification is applied.</param>
        /// <returns>An <see cref="IQueryable{T}"/> filtered according to the specification's predicates.</returns>
        protected static IQueryable<T> ApplyFilterPredicates(
            ISpecification<T> specification,
            IQueryable<T> query
        ) =>
            specification
                .FilterPredicates
                .Aggregate(query, (current, filterPredicate) => current.Where(filterPredicate));

        /// <summary>
        /// Applies eager-loading includes defined in the specification.
        /// </summary>
        /// <param name="specification">The specification that defines eager-loading includes.</param>
        /// <param name="query">The base <see cref="IQueryable{T}"/> to which the specification is applied.</param>
        /// <returns>An <see cref="IQueryable{T}"/> with the specified includes applied for eager loading.</returns>
        protected static IQueryable<T> ApplyIncludes(
            ISpecification<T> specification,
            IQueryable<T> query
        )
        {
            query = specification.Includes.Aggregate(query, (current, include) => include(current));
            return query;
        }

        #endregion

        /// <inheritdoc/>
        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            var createdEntity = await dbContext.AddAsync(entity, cancellationToken);
            return createdEntity.Entity;
        }

        /// <inheritdoc/>
        public async Task<List<T>> GetAllAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
        {
            var query = dbContext.Set<T>().AsQueryable();
            query = ApplyIncludes(specification, query);
            query = ApplyAsSplitQuery(specification, query);
            query = ApplyFilterPredicates(specification, query);
            query = ApplyAsNoTracking(specification, query);
            return await query.ToListAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var entity = dbContext.Set<T>().Local.FirstOrDefault(e => e.Id == id);
            if (entity != null)
            {
                return entity;
            }

            var query = dbContext.Set<T>().AsQueryable();
            return await query.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<(List<T> Items, int TotalCount)> GetPageResultsAsync(
            int page, int pageSize, ISpecification<T>? specification = null, CancellationToken cancellationToken = default)
        {
            var query = dbContext.Set<T>().AsQueryable();
            if (specification != null)
            {
                query = ApplyIncludes(specification, query);
                query = ApplyAsSplitQuery(specification, query);
                query = ApplyFilterPredicates(specification, query);
                query = ApplyAsNoTracking(specification, query);
            }

            var total = await query.CountAsync(cancellationToken);
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
            return (items, total);
        }
    }
}
