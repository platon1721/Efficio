using System.Reflection;
using Efficio.Core.Application.Mappings;
using Microsoft.Extensions.DependencyInjection;

namespace Efficio.Core.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register AutoMapper
        services.AddAutoMapper(typeof(MappingProfile).Assembly);
        
        // Register application services (add these later)
        
        return services;
    }
}