using AutomaticFactoryWithMsDI;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection()
              .AddTransient<UserRepository>()
               //.AddSingleton<ProxyGenerator>()
               //.AddTransient<AutoFactoryInterceptor>()
              .AddAutomaticFactory<IPaymentProcessorFactory>()
              .BuildServiceProvider();

var processorFactory = services.GetRequiredService<IPaymentProcessorFactory>();

var processor = processorFactory.Create("1", "5554", "EUR", 250);

bool result = processor.Process();

Console.WriteLine($"Processed = {result}");
