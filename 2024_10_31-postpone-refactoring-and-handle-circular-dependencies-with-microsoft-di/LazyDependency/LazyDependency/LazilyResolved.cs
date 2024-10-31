using Microsoft.Extensions.DependencyInjection;

namespace LazyDependency;

/// <summary>
/// Dummy type used for lazy DI resolution.
/// </summary>
/// <typeparam name="TService">Type of service to resolve</typeparam>
internal sealed class LazilyResolved<TService> : Lazy<TService>
    where TService : notnull
{
    /// <summary>
    /// This ctor gets service provider injected and gets service from it and passes it down to lazy func value factory for later to resolve.
    /// </summary>
    /// <param name="serviceProvider">Service provider</param>
    public LazilyResolved(IServiceProvider serviceProvider)
        : base(serviceProvider.GetRequiredService<TService>)
    { }
}
