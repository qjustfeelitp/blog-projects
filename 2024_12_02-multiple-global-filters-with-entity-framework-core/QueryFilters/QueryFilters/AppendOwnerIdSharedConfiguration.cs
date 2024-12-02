using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace QueryFilters;

[Service(ServiceLifetime.Transient)]
internal sealed class AppendOwnerIdSharedConfiguration : ISharedConfiguration
{
    private readonly QueryContext db;

    /// <inheritdoc />
    public Type ConfigurationEntityType
    {
        get { return typeof(IOwnedBy); }
    }

    public AppendOwnerIdSharedConfiguration(QueryContext db)
    {
        this.db = db;
    }

    /// <inheritdoc />
    public void Configure(EntityTypeBuilder builder)
    {
        if (builder.Metadata.BaseType is not null)
        {
            return;
        }

        var entityParameter = Expression.Parameter(builder.Metadata.ClrType);
        var efPropertyMethodInfo = typeof(EF).GetMethod(nameof(EF.Property))!.MakeGenericMethod(typeof(int))!;
        var efPropertyCall = Expression.Call(null, efPropertyMethodInfo, entityParameter, Expression.Constant(nameof(IOwnedBy.OwnerId)));

        // get the context
        var dbParameter = Expression.Constant(this.db);

        // setup method info to get the CurrentOwnerIdProvider 
        var dbContextServicesMethodInfo =
            typeof(AccessorExtensions).GetMethod(nameof(AccessorExtensions.GetService), [typeof(IInfrastructure<IServiceProvider>)])!.MakeGenericMethod(typeof(CurrentOwnerIdProvider));

        // get the service
        var dbContextCall = Expression.Call(null, dbContextServicesMethodInfo, dbParameter);

        // get the method info of method that will provide current id / can be property or whatever
        var idGetterMethodInfo = typeof(CurrentOwnerIdProvider).GetMethod(nameof(CurrentOwnerIdProvider.Get))!;

        // get the id
        var idGetterCall = Expression.Call(dbContextCall, idGetterMethodInfo);

        // make the comparison
        var body = Expression.MakeBinary(ExpressionType.Equal, efPropertyCall, idGetterCall);
        var expression = Expression.Lambda(body, entityParameter);

        builder.Metadata.AddOrAppendQueryFilter(expression);
    }
}
