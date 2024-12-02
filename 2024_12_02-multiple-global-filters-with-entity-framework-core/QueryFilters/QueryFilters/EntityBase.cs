namespace QueryFilters;

internal abstract class EntityBase : IEntity
{
    public required int Id { get; init; }
}
