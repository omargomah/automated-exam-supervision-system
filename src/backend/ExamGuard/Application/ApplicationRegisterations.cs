using Application.Behaviors;
using Application.Mappings;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application
{
    public static class ApplicationRegistrations
    {
        public static IServiceCollection AddApplicationRegistrations(this IServiceCollection services)
        {
            // Register MediatR and FluentValidation from the Application assembly
            Assembly assemblyOfApplication = typeof(ApplicationRegistrations).Assembly;
            services.AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(assemblyOfApplication); });
            services.AddValidatorsFromAssembly(assemblyOfApplication);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviors<,>));

            // add Mapster configurations for the Application layer
            services.AddApplicationMappings();
        
            return services;
        }
    }
}
