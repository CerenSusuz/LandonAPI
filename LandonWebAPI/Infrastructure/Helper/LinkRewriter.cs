using LandonWebAPI.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace LandonWebAPI.Infrastructure.Helper;

public class LinkRewriter
{
    private readonly IUrlHelper _urlHelper;

    public LinkRewriter(IUrlHelper urlHelper)
    {
        _urlHelper = urlHelper;
    }

    public Link Rewrite(Link original)
    {
        if (original == null)
        {
            return null;
        }

        return new Link
        {
            Href = _urlHelper.Link(original.RouteName, original.RouteValues),
            Method = original.Method,
            Relations = original.Relations
        };
    }

}