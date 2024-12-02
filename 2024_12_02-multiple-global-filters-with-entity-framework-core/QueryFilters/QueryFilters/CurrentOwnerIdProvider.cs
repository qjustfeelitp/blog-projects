using Microsoft.Extensions.DependencyInjection;

namespace QueryFilters;

[Service(ServiceLifetime.Scoped)]
internal sealed class CurrentOwnerIdProvider
{
    public int Get()
    {
        // read from whatever http header, cookie, claims etc.

        return Random.Shared.Next(1, 101);
    }
}
