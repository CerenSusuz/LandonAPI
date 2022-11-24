using LandonWebAPI.Models.Generic;

namespace LandonWebAPI.Models.DTOs;

public class HotelInfo : Resource
{
    public string Title { get; set; }

    public string Tagline { get; set; }

    public string Email { get; set; }

    public string Website { get; set; }

    public Address Location { get; set; }
}