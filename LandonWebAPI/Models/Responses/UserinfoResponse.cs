using AspNet.Security.OpenIdConnect.Primitives;
using LandonWebAPI.Models.Generic;
using Newtonsoft.Json;

namespace LandonWebAPI.Models.Responses;

public class UserinfoResponse : Resource
{
    [JsonProperty(PropertyName = OpenIdConnectConstants.Claims.Subject)]
    public string Subject { get; set; }

    [JsonProperty(PropertyName = OpenIdConnectConstants.Claims.GivenName)]
    public string GivenName { get; set; }

    [JsonProperty(PropertyName = OpenIdConnectConstants.Claims.FamilyName)]
    public string FamilyName { get; set; }
}
