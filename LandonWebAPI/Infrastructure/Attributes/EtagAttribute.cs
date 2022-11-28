using LandonWebAPI.Infrastructure.Filters;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LandonWebAPI.Infrastructure.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class EtagAttribute : Attribute, IFilterFactory
{
    public bool IsReusable => true;

    public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
    {
        return new EtagHeaderFilter();
    }
}