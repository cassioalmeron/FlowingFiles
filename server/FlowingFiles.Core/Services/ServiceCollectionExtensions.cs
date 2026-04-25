using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace FlowingFiles.Core.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructuralServices(this IServiceCollection services)
    {
        var serviceTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(type => type.Namespace == "FlowingFiles.Core.Services" &&
                          !type.IsAbstract &&
                          !type.IsInterface &&
                          type.IsClass);

        foreach (var serviceType in serviceTypes)
            services.AddScoped(serviceType);

        return services;
    }
}