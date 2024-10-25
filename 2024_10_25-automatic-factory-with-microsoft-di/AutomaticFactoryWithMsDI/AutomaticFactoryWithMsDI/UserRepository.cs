using System.Collections.Concurrent;

namespace AutomaticFactoryWithMsDI;

public sealed record User(string Id);

public sealed class UserRepository
{
    private static readonly ConcurrentDictionary<string, User> Store = new();

    public User GetUser(string id)
    {
        var user = Store.GetOrAdd(id, s => new User(s));

        return user;
    }
}
