using LandonWebAPI.Infrastructure.Feature;

namespace LandonWebAPI.Infrastructure.Extensions;

public static class HttpRequestExtensions
{
    public static IEtagHandlerFeature GetEtagHandler(this HttpRequest request)
        => request.HttpContext.Features.Get<IEtagHandlerFeature>();
}
