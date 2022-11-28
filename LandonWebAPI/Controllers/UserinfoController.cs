using AspNet.Security.OpenIdConnect.Primitives;
using LandonWebAPI.Models.DTOs;
using LandonWebAPI.Models.Responses;
using LandonWebAPI.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LandonWebAPI.Controllers;

[Route("/[controller]")]
[Authorize]
[ApiController]
public class UserinfoController : ControllerBase
{
    private readonly IUserService _userService;

    public UserinfoController(IUserService userService)
    {
        _userService = userService;
    }

    // GET /userinfo
    [HttpGet(Name = nameof(Userinfo))]
    [ProducesResponseType(401)]
    public async Task<ActionResult<UserinfoResponse>> Userinfo()
    {
        var user = await _userService.GetUserAsync(User);
        if (user == null)
        {
            return BadRequest(new OpenIdConnectResponse
            {
                Error = OpenIdConnectConstants.Errors.InvalidGrant,
                ErrorDescription = "The user does not exist."
            });
        }
        var userId = _userService.GetUserIdAsync(User);

        return new UserinfoResponse
        {
            Self = Link.To(nameof(Userinfo)),
            GivenName = user.Firstname,
            FamilyName = user.Lastname,
            Subject = Url.Link(
                nameof(UsersController.GetUserById),
                new { userId })
        };
    }
}
