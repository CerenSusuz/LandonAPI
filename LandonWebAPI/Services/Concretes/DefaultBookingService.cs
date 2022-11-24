using AutoMapper;
using LandonWebAPI.DataAccess;
using LandonWebAPI.Models.DTOs;
using LandonWebAPI.Services.Abstract;
using Microsoft.EntityFrameworkCore;

namespace LandonWebAPI.Services.Concretes;

public class DefaultBookingService : IBookingService
{
    private readonly HotelApiDbContext _context;
    private readonly IMapper _mapper;

    public DefaultBookingService(
        HotelApiDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public Task<Guid> CreateBookingAsync(
        Guid userId,
        Guid roomId,
        DateTimeOffset startAt,
        DateTimeOffset endAt)
    {
        // TODO: Save the new booking to the database
        throw new NotImplementedException();
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