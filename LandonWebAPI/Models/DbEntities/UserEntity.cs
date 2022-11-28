using Microsoft.AspNetCore.Identity;

namespace LandonWebAPI.Models.DbEntities;

public class UserEntity : IdentityUser<Guid>
{
    public string Firstname { get; set; }

    public string Lastname { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
}
