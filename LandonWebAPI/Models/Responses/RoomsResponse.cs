using LandonWebAPI.Models.DTOs;
using LandonWebAPI.Models.Generic;

namespace LandonWebAPI.Models.Responses;

public class RoomsResponse : PagedCollection<Room>
{
    public Link Openings { get; set; }

    public LandonWebAPI.Models.Form.Form RoomsQuery { get; set; }
}