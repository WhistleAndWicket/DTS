using Application.Common.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Persistence
{
    /// <summary>
    /// Factory for creating <see cref="DtsDbContext"/> at design time.
    /// Reads the connection string from the API project's appsettings.json.
    /// </summary>
    public class DtsDbContextFactory : IDesignTimeDbContextFactory<DtsDbContext>
    {
        public DtsDbContext CreateDbContext(string[] args)
        {
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "../Api");

            var config = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .Build();

            var dbOptions = config.GetSection("DatabaseSettings").Get<DatabaseOptions>();

            var optionsBuilder = new DbContextOptionsBuilder<DtsDbContext>();
            optionsBuilder.UseSqlServer(dbOptions?.ConnectionString);

            return new DtsDbContext(optionsBuilder.Options);
        }
    }
}
