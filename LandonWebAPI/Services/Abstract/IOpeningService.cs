using LandonWebAPI.Models.DTOs;
using LandonWebAPI.Models.Generic;
using LandonWebAPI.Models.Options;

namespace LandonWebAPI.Services.Abstract;

public interface IOpeningService
{
    Task<PagedResult<Opening>> GetOpeningsAsync(
        PagingOptions pagingOptions,
        SortOptions<Opening, OpeningEntity> sortOptions);

    Task<IEnumerable<BookingRange>> GetConflictingSlots(
        Guid roomId,
        DateTimeOffset start,
        DateTimeOffset end);
}