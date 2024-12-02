using Microsoft.EntityFrameworkCore;

namespace QueryFilters;

internal abstract class RepositoryBase<TEntity>
    where TEntity : EntityBase, IDeleted, IOwnedBy
{
    protected readonly QueryContext Db;
    protected readonly CurrentOwnerIdProvider CurrentOwnerIdProvider;

    protected RepositoryBase(QueryContext db,
                             CurrentOwnerIdProvider currentOwnerIdProvider)
    {
        this.Db = db;
        this.CurrentOwnerIdProvider = currentOwnerIdProvider;
    }

    public async Task<TEntity?> GetById(int id)
    {
        int currentOwnerId = this.CurrentOwnerIdProvider.Get();

        var entity = await this.Db.Set<TEntity>()
                               .Where(e => !e.Deleted
                                        && (e.OwnerId == currentOwnerId)
                                        && (e.Id == id))
                               .FirstOrDefaultAsync();

        return entity;
    }

    public async Task<TEntity?> GetById2(int id)
    {
        int currentOwnerId = this.CurrentOwnerIdProvider.Get();

        var entity = await this.Db.Set<TEntity>()
                               .NotDeleted()
                               .OwnedBy(currentOwnerId)
                               .Where(e => e.Id == id)
                               .FirstOrDefaultAsync();

        return entity;
    }
}
