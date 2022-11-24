using System.Linq.Expressions;

namespace LandonWebAPI.Infrastructure.Provider;

public interface ISearchExpressionProvider
{
    ConstantExpression GetValue(string input);

    Expression GetComparison(
        MemberExpression left,
        string opr,
        ConstantExpression right);
}
