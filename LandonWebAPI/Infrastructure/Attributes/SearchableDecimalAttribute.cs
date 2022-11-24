using LandonWebAPI.Infrastructure.Provider;

namespace LandonWebAPI.Infrastructure.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class SearchableDecimalAttribute : SearchableAttribute
{
    public SearchableDecimalAttribute()
    {
        ExpressionProvider = new DecimalToIntSearchExpressionProvider();
    }
}
