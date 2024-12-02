using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace QueryFilters;

internal static class DatabaseContextInfrastructureExtensions
{
    /// <summary>
    /// Gets services from db context infrastructure.
    /// </summary>
    /// <typeparam name="TService">Type of service</typeparam>
    /// <param name="accessor">Accessor</param>
    /// <exception cref="InvalidOperationException">If application service provider returns null</exception>
    public static IEnumerable<TService> GetServices<TService>(this IInfrastructure<IServiceProvider> accessor)
        where TService : class
    {
        var internalServiceProvider = accessor.Instance;

        var internalServices = internalServiceProvider.GetServices<TService>().ToArray();

        if (internalServices.Length != 0)
        {
            return internalServices;
        }

        var applicationServices = internalServiceProvider.GetService<IDbContextOptions>()
                                                        ?.Extensions.OfType<CoreOptionsExtension>()
                                                         .FirstOrDefault()
                                                        ?.ApplicationServiceProvider
                                                        ?.GetServices<TService>();

        if (applicationServices is null)
        {
            throw new InvalidOperationException(CoreStrings.NoProviderConfiguredFailedToResolveService(typeof(TService).Name));
        }

        return applicationServices;
    }
}
