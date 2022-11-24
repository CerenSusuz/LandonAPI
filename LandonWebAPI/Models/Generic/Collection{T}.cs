namespace LandonWebAPI.Models.Generic;

public class Collection<T> : Resource
{
    public T[] Value { get; set; }
}