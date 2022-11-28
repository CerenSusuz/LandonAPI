using AutoMapper.QueryableExtensions;
using LandonWebAPI.Models.DbEntities;
using LandonWebAPI.Models.DTOs;
using LandonWebAPI.Models.Form;
using LandonWebAPI.Models.Generic;
using LandonWebAPI.Models.Options;
using LandonWebAPI.Services.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace LandonWebAPI.Services.Concretes;

public class DefaultUserService : IUserService
{
    private readonly UserManager<UserEntity> _userManager;
    private readonly IConfigurationProvider _mappingConfiguration;

    public DefaultUserService(
        UserManager<UserEntity> userManager,
        IConfigurationProvider mappingConfiguration)
    {
        _userManager = userManager;
        _mappingConfiguration = mappingConfiguration;
    }

    public async Task<(bool Succeded, string ErrorMessage)> CreateUserAsync(RegisterForm form)
    {
        var entity = new UserEntity
        {
            Email = form.Email,
            UserName = form.Email,
            Firstname = form.FirstName,
            Lastname = form.LastName,
            CreatedAt = DateTimeOffset.UtcNow
        };

        var result = await _userManager.CreateAsync(entity, form.Password);

        if (!result.Succeeded)
        {
            var firstError = result.Errors.FirstOrDefault()?.Description;

            return (false, firstError);
        }

        return (true, null);


    }

    public async Task<User> GetUserAsync(ClaimsPrincipal user)
    {
        var entity = await _userManager.GetUserAsync(user);
        var mapper = _mappingConfiguration.CreateMapper();

        return mapper.Map<User>(entity);
    }

    public async Task<Guid?> GetUserIdAsync(ClaimsPrincipal principal)
    {
        var user = await _userManager.GetUserAsync(principal);
        if (user == null) return null;

        return user.Id;
    }

    public async Task<PagedResult<User>> GetUsersAsync(
        PagingOptions pagingOptions,
        SortOptions<User, UserEntity> sortOptions,
        SearchOptions<User, UserEntity> searchOptions)
    {
        IQueryable<UserEntity> query = _userManager.Users;
        query = searchOptions.Apply(query);
        query = sortOptions.Apply(query);

        var size = await query.CountAsync();

        var items = await query
            .Skip(pagingOptions.Offset.Value)
            .Take(pagingOptions.Limit.Value)
            .ProjectTo<User>(_mappingConfiguration)
            .ToArrayAsync();

        return new PagedResult<User>
        {
            Items = items,
            TotalSize = size
        };
    }
}
