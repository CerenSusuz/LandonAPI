using LandonWebAPI.Models.DTOs;
using Newtonsoft.Json;

namespace LandonWebAPI.Models.Generic;

public abstract class Resource : Link
{
    [JsonIgnore]
    public Link Self { get; set; }
}