namespace QueryFilters;

internal abstract class PetBase : EntityBase, IDeleted, IOwnedBy
{
    /// <inheritdoc />
    public required bool Deleted { get; init; }

    /// <inheritdoc />
    public required int OwnerId { get; init; }

    /// <inheritdoc />
    public Owner Owner { get; set; } = null!;
}
