using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace QueryFilters;

public interface ISharedConfiguration
{
    /// <summary>
    /// Configuration entity type.
    /// </summary>
    Type ConfigurationEntityType { get; }

    /// <summary>
    /// Configures entity.
    /// </summary>
    /// <param name="builder">Builder</param>
    void Configure(EntityTypeBuilder builder);
}


