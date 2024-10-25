using System.Collections.Concurrent;
using System.Reflection;
using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;

namespace AutomaticFactoryWithMsDI;

/// <summary>
/// Factory interceptor.
/// </summary>
internal class AutoFactoryInterceptor : IInterceptor
{
    private readonly IServiceProvider serviceProvider;
    private static readonly ConcurrentDictionary<MethodInfo, Lazy<ObjectFactory>> Factories = new();

    public AutoFactoryInterceptor(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    /// <inheritdoc />
    public void Intercept(IInvocation invocation)
    {
        var factory = Factories.GetOrAdd(invocation.Method, method => new Lazy<ObjectFactory>(() => CreateFactory(method)));

        invocation.ReturnValue = factory.Value(this.serviceProvider, invocation.Arguments);
    }

    /// <summary>
    /// Creates factory based on method return type and parameters.
    /// </summary>
    /// <param name="method">The method info to create a factory for.</param>
    /// <returns>A delegate that creates the object.</returns>
    private static ObjectFactory CreateFactory(MethodInfo method)
    {
        try
        {
            return ActivatorUtilities.CreateFactory(method.ReturnType, method.GetParameters().Select(p => p.ParameterType).ToArray());
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to create factory for method: {method.Name}", ex);
        }
    }
}
