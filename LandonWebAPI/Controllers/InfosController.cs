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
    public ActionResult<HotelInfo> GetInfo()
    {
        _hotelInfo.Href = Url.Link(nameof(GetInfo), null);

        return _hotelInfo;
    }
}