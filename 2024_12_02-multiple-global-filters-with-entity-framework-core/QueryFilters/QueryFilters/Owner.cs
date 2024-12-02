namespace QueryFilters;

internal sealed class Owner : EntityBase
{
    public ICollection<Cat> Cats { get; } = new HashSet<Cat>();

    public ICollection<Dog> Dogs { get; } = new HashSet<Dog>();
}
