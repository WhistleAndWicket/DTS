using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Domain.Infrastructure.Specifications
{
    /// <summary>
    /// Defines a specification for querying entities of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    public interface ISpecification<T>
    {
        /// <summary>
        /// Gets the collection of filter predicates to be applied to the query.
        /// </summary>
        IEnumerable<Expression<Func<T, bool>>> FilterPredicates => [];

        /// <summary>
        /// Gets the list of include functions to include related entities in the query.
        /// </summary>
        List<Func<IQueryable<T>, IIncludableQueryable<T, object>>> Includes { get; }

        /// <summary>
        /// Gets a value indicating whether to use split query for the includes.
        /// </summary>
        bool UseSplitQuery { get; }

        /// <summary>
        /// Gets a value indicating whether to apply the query with no tracking.
        /// </summary>
        bool ApplyAsNoTracking { get; }
    }
}
