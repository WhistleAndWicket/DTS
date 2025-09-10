using Application.Common.Options;
using Domain.Infrastructure;
using Domain.Infrastructure.Repositories;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Persistence.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    /// <summary>
    /// Provides extension methods for registering infrastructure-related services
    /// into the dependency injection (DI) container, including DbContexts, repositories, and domain services.
    /// </summary>
    public static class IoC
    {
        /// <summary>
        /// Registers the application's database contexts into the DI container.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to register services into.</param>
        /// <param name="configuration">The application configuration used to bind database settings.</param>
        private static void AddDbContexts(IServiceCollection services, ConfigurationManager configuration)
        {
            // Bind the DatabaseSettings
            var dbSettings = configuration.GetSection("DatabaseSettings").Get<DatabaseOptions>();

            // Register DbContext with connection string from settings
            services.AddDbContext<DtsDbContext>(options =>
                options.UseSqlServer(dbSettings.ConnectionString));
        }

        /// <summary>
        /// Registers repository implementations into the DI container.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to register services into.</param>
        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<ITaskItemRepository, TaskItemRepository>();
        }

        /// <summary>
        /// Registers domain and application services into the DI container.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to register services into.</param>
        private static void AddServices(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWorkService>();
        }

        /// <summary>
        /// Configures and registers all infrastructure services for the application.
        /// This includes database contexts, repositories, and domain/application services.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to register services into.</param>
        /// <param name="configuration">The application configuration used to bind settings such as database connection strings.</param>
        /// <returns>The updated <see cref="IServiceCollection"/> for chaining.</returns>
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
        {
            AddDbContexts(services, configuration);
            AddRepositories(services);
            AddServices(services);
            return services;
        }
    }
}
