using LandonWebAPI.Infrastructure.Abstracts;
using LandonWebAPI.Infrastructure.Extensions;
using LandonWebAPI.Models.Generic;
using Newtonsoft.Json;

namespace LandonWebAPI.Models.DTOs;

public class HotelInfo : Resource, IEtaggable
{
    public string Title { get; set; }

    public string Tagline { get; set; }

    public string Email { get; set; }

    public string Website { get; set; }

    public Address Location { get; set; }

    public string GetEtag()
    {
        var serialized = JsonConvert.SerializeObject(this);

        return Md5Hash.ForString(serialized);
    }
}