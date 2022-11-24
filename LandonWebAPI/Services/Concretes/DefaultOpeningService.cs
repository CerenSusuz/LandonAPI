using AutoMapper.QueryableExtensions;
using LandonWebAPI.DataAccess;
using LandonWebAPI.Models.DTOs;
using LandonWebAPI.Models.Generic;
using LandonWebAPI.Models.Options;
using LandonWebAPI.Services.Abstract;
using Microsoft.EntityFrameworkCore;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace LandonWebAPI.Services.Concretes;

public class DefaultOpeningService : IOpeningService
{
    private readonly HotelApiDbContext _context;
    private readonly IDateLogicService _dateLogicService;
    private readonly IConfigurationProvider _mappingProvider;

    public DefaultOpeningService(
        HotelApiDbContext context,
        IDateLogicService dateLogicService,
        IConfigurationProvider mappingProvider)
    {
        _context = context;
        _dateLogicService = dateLogicService;
        _mappingProvider = mappingProvider;
    }

    public async Task<PagedResult<Opening>> GetOpeningsAsync(
                PagingOptions pagingOptions,
                SortOptions<Opening, OpeningEntity> sortOptions)
    {
        var rooms = await _context.Rooms.ToArrayAsync();

        var allOpenings = new List<OpeningEntity>();

        foreach (var room in rooms)
        {
            // Generate a sequence of raw opening slots
            var allPossibleOpenings = _dateLogicService.GetAllSlots(
                    DateTimeOffset.UtcNow,
                    _dateLogicService.FurthestPossibleBooking(DateTimeOffset.UtcNow))
                .ToArray();

            var conflictedSlots = await GetConflictingSlots(
                room.Id,
                allPossibleOpenings.First().StartAt,
                allPossibleOpenings.Last().EndAt);

            // Remove the slots that have conflicts and project
            var openings = allPossibleOpenings
                .Except(conflictedSlots, new BookingRangeComparer())
                .Select(slot => new OpeningEntity
                {
                    RoomId = room.Id,
                    Rate = room.Rate,
                    StartAt = slot.StartAt,
                    EndAt = slot.EndAt
                });

            allOpenings.AddRange(openings);
        }

        var pseudoQuery = allOpenings.AsQueryable();
        pseudoQuery = sortOptions.Apply(pseudoQuery);

        var size = pseudoQuery.Count();

        var items = pseudoQuery
            .Skip(pagingOptions.Offset.Value)
            .Take(pagingOptions.Limit.Value)
            .ProjectTo<Opening>(_mappingProvider)
            .ToArray();

        return new PagedResult<Opening>
        {
            Items = items,
            TotalSize = size
        };
    }

    public async Task<IEnumerable<BookingRange>> GetConflictingSlots(
        Guid roomId,
        DateTimeOffset start,
        DateTimeOffset end)
    {
        return await _context.Bookings
            .Where(b => b.Room.Id == roomId && _dateLogicService.DoesConflict(b, start, end))
            // Split each existing booking up into a set of atomic slots
            .SelectMany(existing => _dateLogicService
                .GetAllSlots(existing.StartAt, existing.EndAt))
            .ToArrayAsync();
    }
}