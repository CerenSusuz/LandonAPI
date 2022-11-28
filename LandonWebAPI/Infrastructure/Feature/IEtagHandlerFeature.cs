using LandonWebAPI.Infrastructure.Abstracts;

namespace LandonWebAPI.Infrastructure.Feature;

public interface IEtagHandlerFeature
{
    bool NoneMatch(IEtaggable entity);
}