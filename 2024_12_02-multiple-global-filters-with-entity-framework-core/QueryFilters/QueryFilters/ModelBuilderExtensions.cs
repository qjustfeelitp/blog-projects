using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace QueryFilters;

internal static class ModelBuilderExtensions
{
    /// <summary>
    /// Applies shared entity type configurations.
    /// </summary>
    /// <param name="modelBuilder">Model builder</param>
    /// <param name="configurations">Configurations</param>
    /// <returns></returns>
    public static ModelBuilder ApplySharedEntityTypeConfigurations(this ModelBuilder modelBuilder, IEnumerable<ISharedConfiguration> configurations)
    {
        foreach (var configuration in configurations)
        {
            foreach (var entityType in modelBuilder.FilterTypes(configuration.ConfigurationEntityType))
            {
                var entityBuilderType = typeof(EntityTypeBuilder<>).MakeGenericType(entityType.ClrType);
                var entityTypeBuilder = (EntityTypeBuilder)Activator.CreateInstance(entityBuilderType, entityType)!;

                configuration.Configure(entityTypeBuilder);
            }
        }

        return modelBuilder;
    }

    /// <summary>
    /// Filters types.
    /// </summary>
    /// <param name="modelBuilder">Model builder</param>
    /// <param name="type">Type</param>
    private static IEnumerable<IMutableEntityType> FilterTypes(this ModelBuilder modelBuilder, Type type)
    {
        return modelBuilder.Model.GetEntityTypes().Where(x => type.IsAssignableFrom(x.ClrType));
    }
}
