using LandonWebAPI.Infrastructure.Provider;

namespace LandonWebAPI.Infrastructure.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class SearchableAttribute : Attribute
{
    public ISearchExpressionProvider ExpressionProvider { get; set; }
    = new DefaultSearchExpressionProvider();
}