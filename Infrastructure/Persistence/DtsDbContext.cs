using Domain.Entities.Dts;
using Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    /// <summary>
    /// DbContext for the DTS application.
    /// Handles the database connection and entity mapping.
    /// </summary>
    /// <param name="options">The options to configure the DbContext.</param>
    public class DtsDbContext(DbContextOptions<DtsDbContext> options) : DbContext(options)
    {
        /// <summary>
        /// The <see cref="TaskItem"/> entities in the database. Represents the "Tasks" table.
        /// </summary>
        public virtual DbSet<TaskItem> Tasks { get; set; }

        /// <inheritdoc/>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TaskItemConfiguration());
        }
    }
}
