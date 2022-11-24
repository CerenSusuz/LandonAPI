using LandonWebAPI.Models.DTOs;
using LandonWebAPI.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace LandonWebAPI.Controllers;

[Route("/")]
[ApiController]
[ApiVersion("1.0")]
public class RootController : ControllerBase
{
    [HttpGet(Name = nameof(GetRoot))]
    [ProducesResponseType(200)]
    public IActionResult GetRoot()
    {
        var response = new RootResponse
        {
            Self = Link.To(nameof(GetRoot)),
            Rooms = Link.ToCollection(nameof(RoomsController.GetAllRooms)),
            Info = Link.To(nameof(InfosController.GetInfo))
        };

        return Ok(response);
    }
}