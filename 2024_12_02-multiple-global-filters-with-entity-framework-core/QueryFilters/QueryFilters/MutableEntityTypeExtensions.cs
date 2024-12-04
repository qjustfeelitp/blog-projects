using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;

namespace QueryFilters;

public static class MutableEntityTypeExtensions
{
    /// <summary>
    /// Adds or appends query filter to EF entity type.
    /// </summary>
    /// <param name="mutableEntityType">Mutable entity type</param>
    /// <param name="expression">Expression</param>
    public static void AddOrAppendQueryFilter(this IMutableEntityType mutableEntityType, LambdaExpression expression)
    {
        var parameterType = Expression.Parameter(mutableEntityType.ClrType);

        var expressionFilter = ReplacingExpressionVisitor.Replace(expression.Parameters.Single(), parameterType, expression.Body);

        var existingFilter = mutableEntityType.GetQueryFilter();

        if (existingFilter is not null)
        {
            var currentExpressionFilter = ReplacingExpressionVisitor.Replace(existingFilter.Parameters.Single(), parameterType, existingFilter.Body);
            expressionFilter = Expression.AndAlso(currentExpressionFilter, expressionFilter);
        }

        var lambdaExpression = Expression.Lambda(expressionFilter, parameterType);
        mutableEntityType.SetQueryFilter(lambdaExpression);
    }

    public static void AddOrAppendQueryFilter(this IConventionEntityType conventionEntityType, LambdaExpression expression)
    {
        var parameterType = Expression.Parameter(conventionEntityType.ClrType);

        var expressionFilter = ReplacingExpressionVisitor.Replace(expression.Parameters.Single(), parameterType, expression.Body);

        var existingFilter = conventionEntityType.GetQueryFilter();

        if (existingFilter is not null)
        {
            var currentExpressionFilter = ReplacingExpressionVisitor.Replace(existingFilter.Parameters.Single(), parameterType, existingFilter.Body);
            expressionFilter = Expression.AndAlso(currentExpressionFilter, expressionFilter);
        }

        var lambdaExpression = Expression.Lambda(expressionFilter, parameterType);
        conventionEntityType.SetQueryFilter(lambdaExpression);
    }
}
