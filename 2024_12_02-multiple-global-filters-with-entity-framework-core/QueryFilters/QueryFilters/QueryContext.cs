using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace QueryFilters;

internal sealed class QueryContext : DbContext
{
    public DbSet<Cat> Cats
    {
        get { return Set<Cat>(); }
    }

    public DbSet<Dog> Dogs
    {
        get { return Set<Dog>(); }
    }

    public DbSet<Owner> Owners
    {
        get { return Set<Owner>(); }
    }

    public QueryContext(DbContextOptions<QueryContext> options) : base(options)
    { }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //modelBuilder.Entity<Cat>()
        //            .HasQueryFilter(e => !e.Deleted);

        //modelBuilder.Entity<Dog>()
        //            .HasQueryFilter(e => !e.Deleted);

        var configurations = this.GetServices<ISharedConfiguration>();

        //modelBuilder.ApplySharedEntityTypeConfigurations(configurations);
    }

    /// <inheritdoc />
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        //var conventions = this.GetServices<IModelFinalizingConvention>();

        //foreach (var convention in conventions)
        //{
        //    configurationBuilder.Conventions.Add(_ => convention);
        //}

        //configurationBuilder.Conventions.Add(provider => provider.GetRequiredService<ICurrentDbContext>().Context.GetService<AppendOwnerIdConvention>());
        //configurationBuilder.Conventions.Add(provider => provider.GetRequiredService<ICurrentDbContext>().Context.GetService<DeletedConvention>());

        configurationBuilder.Conventions.Add(provider => provider.GetRequiredService<AppendOwnerIdConvention>());
        configurationBuilder.Conventions.Add(provider => provider.GetRequiredService<DeletedConvention>());
    }
}
