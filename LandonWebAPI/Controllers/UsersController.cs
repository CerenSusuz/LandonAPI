using LandonWebAPI.Models.DbEntities;
using LandonWebAPI.Models.DTOs;
using LandonWebAPI.Models.Form;
using LandonWebAPI.Models.Generic;
using LandonWebAPI.Models.Options;
using LandonWebAPI.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LandonWebAPI.Controllers;

[Route("/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly PagingOptions _defaultPagingOptions;
    private readonly IAuthorizationService _authService;

    public UsersController(
        IUserService userService,
        IAuthorizationService authService,
        IOptions<PagingOptions> defaultPagingOptions)
    {
        _userService = userService;
        _authService = authService;
        _defaultPagingOptions = defaultPagingOptions.Value;
    }

    [HttpGet(Name = nameof(GetVisibleUsers))]
    public async Task<ActionResult<PagedCollection<User>>> GetVisibleUsers(
        [FromQuery] PagingOptions pagingOptions,
        [FromQuery] SortOptions<User, UserEntity> sortOptions,
        [FromQuery] SearchOptions<User, UserEntity> searchOptions)
    {
        pagingOptions.Offset = pagingOptions.Offset ?? _defaultPagingOptions.Offset;
        pagingOptions.Limit = pagingOptions.Limit ?? _defaultPagingOptions.Limit;

        var users = new PagedResult<User>();

        if (User.Identity.IsAuthenticated)
        {
            var canSeeEveryone = await _authService.AuthorizeAsync(
                User, "ViewAllUsersPolicy");

            if (canSeeEveryone.Succeeded)
            {
                users = await _userService.GetUsersAsync(
                    pagingOptions, sortOptions, searchOptions);
            }
            else
            {
                var myself = await _userService.GetUserAsync(User);
                users.Items = new[] { myself };
                users.TotalSize = 1;
            }
        }

        var collection = PagedCollection<User>.Create(
            Link.ToCollection(nameof(GetVisibleUsers)),
            users.Items.ToArray(),
            users.TotalSize,
            pagingOptions);

        return collection;
    }

    [Authorize]
    [ProducesResponseType(401)]
    [HttpGet("{userId}", Name = nameof(GetUserById))]
    public Task<IActionResult> GetUserById(Guid userId)
    {
        // TODO is userId the current user's ID?
        // If so, return myself.
        // If not, only Admin roles should be able to view arbitrary users.
        throw new NotImplementedException();
    }

    [HttpPost(Name = nameof(RegisterUser))]
    [ProducesResponseType(400)]
    [ProducesResponseType(201)]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterForm form)
    {
        var (succeded, message) = await _userService.CreateUserAsync(form);

        if (succeded)
        {
            return Created(
               Url.Link(nameof(UserinfoController.Userinfo), null),
               null);
        }

        return BadRequest(new ApiError
        {
            Message = " Registration Failed",
            Detail = message
        });
    }
}