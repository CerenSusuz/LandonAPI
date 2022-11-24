using LandonWebAPI.Infrastructure.Provider;

namespace LandonWebAPI.Infrastructure.Terms;

public class SearchTerm
{
    public string Name { get; set; }

    public string Operator { get; set; }

    public string Value { get; set; }

    public bool IsValidSyntax { get; set; }

    public ISearchExpressionProvider ExpressionProvider { get; set; }
}