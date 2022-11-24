namespace LandonWebAPI.Models.Generic;

public class PagedResult<T>
{
    public IEnumerable<T> Items { get; set; }

    public int TotalSize { get; set; }
}
