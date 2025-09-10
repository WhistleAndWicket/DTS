using Application.Common.Behaviours;
using Application.Features.TaskItem.Commands.Create;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    /// <summary>
    /// Provides extension methods for registering application-level services
    /// into the dependency injection (DI) container.
    /// </summary>
    public static class IoC
    {
        /// <summary>
        /// Registers the application services, MediatR handlers, and pipeline behaviors
        /// into the specified <paramref name="services"/>.
        /// </summary>
        /// <param name="services">The service collection to add the application services to.</param>
        /// <returns>The updated <see cref="IServiceCollection"/> instance, enabling fluent chaining.</returns>
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Add MediatR.
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblyContaining<CreateTaskItemCommandHandler>();
            });

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(QueryBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CommandBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehaviour<,>));

            return services;
        }
    }
}
