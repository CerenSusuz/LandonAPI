using LandonWebAPI.Models.Generic;

namespace LandonWebAPI.Models.DTOs;

public class User : Resource
{
    public string Email { get; set; }

    public string Firstname { get; set; }

    public string Lastname { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
}
