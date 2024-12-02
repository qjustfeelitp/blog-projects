using Microsoft.EntityFrameworkCore;

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

        modelBuilder.ApplySharedEntityTypeConfigurations(configurations);
    }
}
