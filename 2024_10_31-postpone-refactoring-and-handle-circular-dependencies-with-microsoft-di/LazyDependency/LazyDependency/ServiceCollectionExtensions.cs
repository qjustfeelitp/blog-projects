using Microsoft.Extensions.DependencyInjection;

namespace LazyDependency;

internal static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds lazy resolution. Used to workaround cyclomatic dependencies.
    /// </summary>
    /// <param name="serviceCollection">Service collection</param>
    /// <remarks>Usage: inject Lazy{T} to constructor, store it in field and access Lazy.Value from property or access it directly.</remarks>
    public static IServiceCollection AddLazyResolution(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddTransient(typeof(Lazy<>),
                                              typeof(LazilyResolved<>));
    }
}
