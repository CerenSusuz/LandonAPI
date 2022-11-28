using LandonWebAPI.Infrastructure.Attributes;
using LandonWebAPI.Infrastructure.Extensions;
using LandonWebAPI.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LandonWebAPI.Controllers;

[Route("/[controller]")]
[ApiController]
public class InfosController : ControllerBase
{
    private readonly HotelInfo _hotelInfo;

    public InfosController(IOptions<HotelInfo> hotelInfoWrapper)
    {
        _hotelInfo = hotelInfoWrapper.Value;
    }

    [HttpGet(Name = nameof(GetInfo))]
    [ProducesResponseType(200)]
    [ProducesResponseType(304)]
    [ResponseCache(CacheProfileName = "Static")]
    [Etag]
    public ActionResult<HotelInfo> GetInfo()
    {
        _hotelInfo.Href = Url.Link(nameof(GetInfo), null);

        if (!Request.GetEtagHandler().NoneMatch(_hotelInfo))
        {
            return StatusCode(304, _hotelInfo);
        }
        return _hotelInfo;
    }
}