using LandonWebAPI.DataAccess;
using LandonWebAPI.Models.DbEntities;
using Microsoft.AspNetCore.Identity;

namespace LandonWebAPI.CoreServices;

public static class CoreServices
{
    public static void AddIdentityCoreServices(IServiceCollection services)
    {
        var builder = services.AddIdentityCore<UserEntity>();
        builder = new IdentityBuilder(
            builder.UserType,
            typeof(UserRoleEntity),
            builder.Services);

        builder.AddRoles<UserRoleEntity>()
            .AddEntityFrameworkStores<HotelApiDbContext>()
            .AddDefaultTokenProviders()
            .AddSignInManager<SignInManager<UserEntity>>();
    }
}
