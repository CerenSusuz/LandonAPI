using LandonWebAPI.Models.DTOs;
using LandonWebAPI.Models.Generic;

namespace LandonWebAPI.Models.Responses;

public class RootResponse : Resource
{
    public Link Info { get; set; }

    public Link Rooms { get; set; }
}