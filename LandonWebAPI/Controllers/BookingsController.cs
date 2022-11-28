using LandonWebAPI.Models.DTOs;
using LandonWebAPI.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace LandonWebAPI.Controllers;

[Route("/[controller]")]
[ApiController]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingsController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpGet("{bookingId}", Name = nameof(GetBookingById))]
    [ProducesResponseType(404)]
    [ProducesResponseType(200)]
    public async Task<ActionResult<Booking>> GetBookingById(Guid bookingId)
    {
        var booking = await _bookingService.GetBookingAsync(bookingId);
        if (booking == null)
        {
            return NotFound();
        }
            
        return booking;
    }

    [HttpDelete("{bookingId", Name = nameof(DeleteBookingId))]
    public async Task<IActionResult> DeleteBookingId(Guid bookingId)
    {
        await _bookingService.DeleteBookingAsync(bookingId);

        return NoContent();
    }
}