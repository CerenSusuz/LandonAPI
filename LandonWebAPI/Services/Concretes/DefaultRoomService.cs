using AutoMapper.QueryableExtensions;
using LandonWebAPI.DataAccess;
using LandonWebAPI.Models.DbEntities;
using LandonWebAPI.Models.DTOs;
using LandonWebAPI.Models.Generic;
using LandonWebAPI.Models.Options;
using LandonWebAPI.Services.Abstract;
using Microsoft.EntityFrameworkCore;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace LandonWebAPI.Services.Concretes;

public class DefaultRoomService : IRoomService
{
    private readonly HotelApiDbContext _context;
    private readonly IConfigurationProvider _mappingProvider;

    public DefaultRoomService(
        HotelApiDbContext context,
        IConfigurationProvider mappingProvider)
    {
        _context = context;
        _mappingProvider = mappingProvider;
    }

    public async Task<PagedResult<Room>> GetAllRoomsAsync(
        PagingOptions pagingOptions,
        SortOptions<Room, RoomEntity> sortOptions,
        SearchOptions<Room, RoomEntity> searchOptions)
    {
        IQueryable<RoomEntity> query = _context.Rooms;
        query = searchOptions.Apply(query);
        query = sortOptions.Apply(query);

        var size = await query.CountAsync();

        var items = await query
            .Skip(pagingOptions.Offset.Value)
            .Take(pagingOptions.Limit.Value)
            .ProjectTo<Room>(_mappingProvider)
            .ToArrayAsync();

        return new PagedResult<Room>
        {
            Items = items,
            TotalSize = size
        };
    }

    public async Task<Room> GetRoomAsync(Guid id)
    {
        var entity = await _context.Rooms
         .SingleOrDefaultAsync(entity => entity.Id == id);

        if (entity == null)
        {
            return null;
        }

        var mapper = _mappingProvider.CreateMapper();

        return mapper.Map<Room>(entity);
    }
}