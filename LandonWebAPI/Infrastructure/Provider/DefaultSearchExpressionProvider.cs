using System.Linq.Expressions;

namespace LandonWebAPI.Infrastructure.Provider;

public class DefaultSearchExpressionProvider : ISearchExpressionProvider
{
    protected const string EqualsOperator = "eq";

    public virtual IEnumerable<string> GetOperators()
    {
        yield return EqualsOperator;
    }

    public virtual Expression GetComparison(
        MemberExpression left,
        string opr,
        ConstantExpression right)
    {
        if (!opr.Equals("eq", StringComparison.OrdinalIgnoreCase))
        {
            throw new ArgumentException($"Invalid operator: '{opr}'.");
        }

        return Expression.Equal(left, right);
    }

    public virtual ConstantExpression GetValue(string input)
               => Expression.Constant(input);
}