using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QueryFilters;

var applicationBuilder = new HostApplicationBuilder(args);

applicationBuilder.Services
                  .AddEntityFrameworkSqlite()
                  .AddDbContext<QueryContext>((provider, builder) => builder.UseSqlite("Data Source=db.db;").UseInternalServiceProvider(provider))
                  .AddServices();

var host = applicationBuilder.Build();

await using (var scope = host.Services.CreateAsyncScope())
{
    await scope.ServiceProvider.GetRequiredService<DbSeeder>().Seed();
}

await using (var scope = host.Services.CreateAsyncScope())
{
    var db = scope.ServiceProvider.GetRequiredService<QueryContext>();

    string q = db.Cats.ToQueryString();
}

