using Domain.Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore.Query;

namespace Application.Common.Specifications
{
    /// <summary>
    /// Represents the default implementation of the <see cref="ISpecification{TEntity}"/> interface.
    /// Provides a base specification with no includes, split query disabled, and optional tracking control.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity this specification applies to.</typeparam>
    public class DefaultSpecification<TEntity> : ISpecification<TEntity>
    {
        /// <summary>
        /// Gets the collection of include expressions that define 
        /// the related entities to include in the query results.
        /// Defaults to an empty list.
        /// </summary>
        public List<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>> Includes => [];

        /// <summary>
        /// Gets a value indicating whether the query should be executed as a split query.
        /// Defaults to <c>false</c>.
        /// </summary>
        public bool UseSplitQuery => false;

        /// <summary>
        /// Gets or sets a value indicating whether the query should be executed
        /// with change tracking disabled (<c>AsNoTracking()</c>).
        /// Defaults to <c>false</c>.
        /// </summary>
        public bool ApplyAsNoTracking { get; set; } = false;
    }
}
