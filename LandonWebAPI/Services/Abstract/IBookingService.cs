using LandonWebAPI.Models.DTOs;

namespace LandonWebAPI.Services.Abstract;

public interface IBookingService
{
    Task<Booking> GetBookingAsync(Guid bookingId);

    Task<Guid> CreateBookingAsync(
        Guid userId,
        Guid roomId,
        DateTimeOffset startAt,
        DateTimeOffset endAt);
}