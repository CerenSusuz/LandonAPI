using LandonWebAPI.Models.DbEntities;
using LandonWebAPI.Models.DTOs;
using LandonWebAPI.Models.Form;
using LandonWebAPI.Models.Generic;
using LandonWebAPI.Models.Options;
using System.Security.Claims;

namespace LandonWebAPI.Services.Abstract;

public interface IUserService
{
    Task<PagedResult<User>> GetUsersAsync(
        PagingOptions pagingOptions,
        SortOptions<User, UserEntity> sortOptions,
        SearchOptions<User, UserEntity> searchOptions);

    Task<(bool Succeded, string ErrorMessage)> CreateUserAsync(RegisterForm form);

    Task<Guid?> GetUserIdAsync(ClaimsPrincipal principal);

    Task<User> GetUserAsync(ClaimsPrincipal user);
}