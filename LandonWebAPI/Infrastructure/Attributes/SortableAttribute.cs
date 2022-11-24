namespace LandonWebAPI.Infrastructure.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class SortableAttribute : Attribute
{
    public bool Default { get; set; }
}
