using Microsoft.AspNetCore.Identity;

namespace LandonWebAPI.Models.DbEntities;

public class UserRoleEntity : IdentityRole<Guid>
{
    public UserRoleEntity() 
        : base()
    {

    }

    public UserRoleEntity(string roleName) 
        : base(roleName)
    {

    }
}
