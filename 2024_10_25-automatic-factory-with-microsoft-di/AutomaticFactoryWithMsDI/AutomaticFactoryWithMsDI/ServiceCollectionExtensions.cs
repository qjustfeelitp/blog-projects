using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;

namespace AutomaticFactoryWithMsDI;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAutomaticFactory<T>(this IServiceCollection serviceCollection, ServiceLifetime lifetime = ServiceLifetime.Transient)
    {
        var type = typeof(T);

        var serviceDescriptor = ServiceDescriptor.Describe(type,
                                                           serviceProvider => serviceProvider.GetRequiredService<ProxyGenerator>()
                                                                                             .CreateInterfaceProxyWithoutTarget(type, serviceProvider.GetRequiredService<AutoFactoryInterceptor>()),
                                                           lifetime);

        serviceCollection.Add(serviceDescriptor);

        return serviceCollection;
    }
}
