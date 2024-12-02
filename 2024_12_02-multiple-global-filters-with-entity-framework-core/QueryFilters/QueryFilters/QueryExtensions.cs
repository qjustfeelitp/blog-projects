namespace QueryFilters;

internal static class QueryExtensions
{
    public static IQueryable<TEntity> NotDeleted<TEntity>(this IQueryable<TEntity> query)
        where TEntity : IDeleted
    {
        return query.Where(e => !e.Deleted);
    }

    public static IQueryable<TEntity> OnlyDeleted<TEntity>(this IQueryable<TEntity> query)
        where TEntity : IDeleted
    {
        return query.Where(e => e.Deleted);
    }

    public static IQueryable<TEntity> OwnedBy<TEntity>(this IQueryable<TEntity> query, int ownerId)
        where TEntity : IOwnedBy
    {
        return query.Where(e => e.OwnerId == ownerId);
    }
}
