using System.Linq.Expressions;
using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace QueryFilters;

[Service(ServiceLifetime.Scoped)]
internal sealed class DbSeeder
{
    private readonly QueryContext db;

    public DbSeeder(QueryContext db)
    {
        this.db = db;
    }

    public async Task Seed()
    {
        if (await this.db.Owners.AnyAsync())
        {
            return;
        }

        await this.db.Database.EnsureDeletedAsync();
        await this.db.Database.MigrateAsync();

        int catIds = 1;

        var catFaker = new Faker<Cat>()
                      .RuleFor(c => c.Id, () => catIds++)
                      .RuleFor(c => c.Deleted, faker => faker.Random.Bool());

        int dogIds = 1;

        var dogFaker = new Faker<Dog>()
                      .RuleFor(i => i.Id, () => dogIds++)
                      .RuleFor(i => i.Deleted, faker => faker.Random.Bool());

        int ownerIds = 1;

        var ownerFaker = new Faker<Owner>()
                        .RuleFor(user => user.Id, () => ownerIds++)
                        .RuleForICollection(o => o.Cats, _ => catFaker.GenerateBetween(0, 5))
                        .RuleForICollection(o => o.Dogs, _ => dogFaker.GenerateBetween(0, 5));

        var owners = ownerFaker.Generate(100);
        this.db.Set<Owner>().AddRange(owners);
        await this.db.SaveChangesAsync();
    }
}

internal static class FakerExtensions
{
    public static Faker<T> RuleForICollection<T, U>(this Faker<T> fakerT, Expression<Func<T, ICollection<U>>> propertyOfListU, Func<Faker, ICollection<U>> itemsGetter)
        where T : class
    {
        var func = propertyOfListU.Compile();

        fakerT.RuleFor(propertyOfListU, (f, t) =>
        {
            var collection = func(t);
            var items = itemsGetter(f);

            foreach (var item in items)
            {
                collection.Add(item);
            }

            return collection;
        });

        return fakerT;
    }
}
