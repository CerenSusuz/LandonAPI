using AutoMapper;
using LandonWebAPI.DataAccess;
using LandonWebAPI.Models.DbEntities;
using LandonWebAPI.Models.DTOs;
using LandonWebAPI.Services.Abstract;
using Microsoft.EntityFrameworkCore;

namespace LandonWebAPI.Services.Concretes;

public class DefaultBookingService : IBookingService
{
    private readonly HotelApiDbContext _context;
    private readonly IDateLogicService _dateLogicService;
    private readonly IMapper _mapper;

    public DefaultBookingService(
        HotelApiDbContext context,
        IDateLogicService dateLogicService,
        IMapper mapper)
    {
        _context = context;
        _dateLogicService = dateLogicService;
        _mapper = mapper;
    }

    public async Task<Guid> CreateBookingAsync(
        Guid userId,
        Guid roomId,
        DateTimeOffset startAt,
        DateTimeOffset endAt)
    {
        var room = await _context.Rooms
            .SingleOrDefaultAsync(room => room.Id == roomId);

        if (room == null)
        {
            throw new ArgumentException("Invalid room ID");
        }

        var minimumStay = _dateLogicService.GetMinimumStay();
        var total = (int)((endAt - startAt).TotalHours / minimumStay.TotalHours)
            * room.Rate;

        var id = Guid.NewGuid();

        var newBooking = _context.Bookings.Add(new BookingEntity
        {
            Id = id,
            CreatedAt = DateTimeOffset.UtcNow,
            ModifiedAt = DateTimeOffset.UtcNow,
            StartAt = startAt.ToUniversalTime(),
            EndAt = endAt.ToUniversalTime(),
            Total = total,
            Room = room
        });

        var created = await _context.SaveChangesAsync();

        if (created < 1)
        {
            throw new InvalidOperationException("Could not create booking");
        }

        return id;
    }

    public async Task<Booking> GetBookingAsync(Guid bookingId)
    {
        var entity = await _context.Bookings
            .SingleOrDefaultAsync(b => b.Id == bookingId);

        if (entity == null)
        {
            return null;
        }

        return _mapper.Map<Booking>(entity);
    }
}