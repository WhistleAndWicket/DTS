namespace Domain.Infrastructure
{
    /// <summary>
    /// Defines a unit of work that encapsulates database operations.
    /// Ensures that changes made through repositories are persisted
    /// as a single atomic operation.
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Saves the changes to the database.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to use.</param>
        /// <returns>Number of entries written to the database.</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
