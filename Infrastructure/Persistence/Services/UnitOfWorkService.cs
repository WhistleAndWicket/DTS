using Domain.Infrastructure;

namespace Infrastructure.Persistence.Services
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="dbContext">The DbContext for the application.</param>
    public class UnitOfWorkService(DtsDbContext dbContext) : IUnitOfWork
    {
        /// <summary>
        /// The DbContext for the application.
        /// </summary>
        private readonly DtsDbContext _dbContext = dbContext;        

        /// <inheritdoc/>
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
