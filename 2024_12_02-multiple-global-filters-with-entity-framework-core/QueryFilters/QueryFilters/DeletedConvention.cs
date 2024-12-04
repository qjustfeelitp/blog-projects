using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.DependencyInjection;

namespace QueryFilters;

[Service(ServiceLifetime.Scoped)]
internal sealed class DeletedConvention : IModelFinalizingConvention
{
    /// <inheritdoc />
    public void ProcessModelFinalizing(IConventionModelBuilder modelBuilder, IConventionContext<IConventionModelBuilder> context)
    {
        var types = modelBuilder.Metadata.GetEntityTypes()
                                .Where(x => typeof(IDeleted).IsAssignableFrom(x.ClrType));

        foreach (var type in types)
        {
            var builder = type.Builder;

            if (builder.Metadata.BaseType is not null)
            {
                continue;
            }

            // the entity type
            var entityParameter = Expression.Parameter(builder.Metadata.ClrType);

            // ef method to get the property
            var efPropertyMethodInfo = typeof(EF).GetMethod(nameof(EF.Property))!.MakeGenericMethod(typeof(bool))!;

            // call the ef method with what property to get
            var efPropertyCall = Expression.Call(null, efPropertyMethodInfo, entityParameter, Expression.Constant(nameof(IDeleted.Deleted)));

            // constant to exclude the entities that have property set to false in this case
            var exclude = Expression.Constant(false);

            // make a comparison 
            var body = Expression.MakeBinary(ExpressionType.Equal, efPropertyCall, exclude);

            // setup the final lambda
            var expression = Expression.Lambda(body, entityParameter);

            // call our extension method to add or append to existing query filter
            builder.Metadata.AddOrAppendQueryFilter(expression);
        }
    }
}
