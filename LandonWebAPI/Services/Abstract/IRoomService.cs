using LandonWebAPI.Models.DbEntities;
using LandonWebAPI.Models.DTOs;
using LandonWebAPI.Models.Generic;
using LandonWebAPI.Models.Options;

namespace LandonWebAPI.Services.Abstract;

public interface IRoomService
{
    Task<Room> GetRoomAsync(Guid id);

    Task<PagedResult<Room>> GetAllRoomsAsync(
        PagingOptions pagingOptions,
        SortOptions<Room, RoomEntity> sortOptions,
        SearchOptions<Room, RoomEntity> searchOptions);
}