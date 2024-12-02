namespace QueryFilters;

internal interface IOwnedBy
{
    int OwnerId { get; }
    Owner Owner { get; }
}
