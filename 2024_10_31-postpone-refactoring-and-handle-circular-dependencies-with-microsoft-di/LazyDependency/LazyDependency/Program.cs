using LazyDependency;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection()
              .AddLazyResolution()
              .AddTransient<Bad.A>()
              .AddTransient<Bad.B>()
              .BuildServiceProvider(new ServiceProviderOptions
               {
                   ValidateOnBuild = true,
                   ValidateScopes = true
               });

var a = services.GetRequiredService<Bad.A>();

var servicesFromAttributes = new ServiceCollection()
                            .AddServices()
                            .BuildServiceProvider(new ServiceProviderOptions
                             {
                                 ValidateOnBuild = true,
                                 ValidateScopes = true
                             });

var c = servicesFromAttributes.GetRequiredService<Bad.C>();