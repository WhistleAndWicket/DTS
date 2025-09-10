using Infrastructure.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Tests.Unit.Core
{
    /// <summary>
    /// Base class for repository tests using SQLite in-memory database.
    /// Provides setup and teardown logic for creating a <see cref="DtsDbContext"/>.
    /// </summary>
    public class RepositoryTestBase : IDisposable
    {
        /// <summary>
        /// Represents a connection to the SQLite database.
        /// </summary>
        private readonly SqliteConnection _connection;

        /// <summary>
        /// The database context for the DTS application.
        /// </summary>
        protected readonly DtsDbContext Context;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryTestBase"/> class.
        /// </summary>
        protected RepositoryTestBase()
        {
            // Create an in-memory SQLite connection.
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<DtsDbContext>()
                .UseSqlite(_connection)
                .Options;

            Context = new DtsDbContext(options);

            // Ensure schema is created.
            Context.Database.EnsureCreated();
        }

        /// <summary>
        /// Cleans up the in-memory database and closes the connection.
        /// </summary>
        public void Dispose()
        {
            Context?.Dispose();
            _connection?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
