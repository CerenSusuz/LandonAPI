namespace LandonWebAPI.Infrastructure.Abstracts;

public interface IEtaggable
{
    string GetEtag();
}
