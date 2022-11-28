using LandonWebAPI.Infrastructure.Attributes;
using LandonWebAPI.Models.Generic;

namespace LandonWebAPI.Models.DTOs;

public class Room : Resource
{
    [Sortable]
    [Searchable]
    public string Name { get; set; }

    [Sortable(Default = true)]
    [SearchableDecimal]
    public decimal Rate { get; set; }

    public LandonWebAPI.Models.Form.Form Book { get; set; }
}