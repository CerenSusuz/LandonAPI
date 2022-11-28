using LandonWebAPI.Infrastructure;
using LandonWebAPI.Models.DbEntities;
using LandonWebAPI.Models.DTOs;
using LandonWebAPI.Models.Form;
using LandonWebAPI.Models.Generic;
using LandonWebAPI.Models.Options;
using LandonWebAPI.Models.Responses;
using LandonWebAPI.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LandonWebAPI.Controllers;

[Route("/[controller]")]
[ApiController]
public class RoomsController : ControllerBase
{
    private readonly IRoomService _roomService;
    private readonly IOpeningService _openingService;
    private readonly IBookingService _bookingService;
    private readonly IDateLogicService _dateLogicService;
    private readonly PagingOptions _defaultPagingOptions;

    public RoomsController(
        IRoomService roomService,
        IOpeningService openingService,
        IBookingService bookingService,
        IDateLogicService dateLogicService,
        IOptions<PagingOptions> defaultPagingOptionsWrapper)
    {
        _roomService = roomService;
        _openingService = openingService;
        _bookingService = bookingService;
        _dateLogicService = dateLogicService;
        _defaultPagingOptions = defaultPagingOptionsWrapper.Value;
    }

    [HttpGet(Name = nameof(GetAllRooms))]
    [ProducesResponseType(200)]
    public async Task<ActionResult<Collection<Room>>> GetAllRooms(
        [FromQuery] PagingOptions pagingOptions,
        [FromQuery] SortOptions<Room, RoomEntity> sortOptions,
        [FromQuery] SearchOptions<Room, RoomEntity> searchOption)
    {
        pagingOptions.Offset = pagingOptions.Offset ?? _defaultPagingOptions.Offset;
        pagingOptions.Limit = pagingOptions.Limit ?? _defaultPagingOptions.Limit;

        var rooms = await _roomService.GetAllRoomsAsync(pagingOptions, sortOptions, searchOption);

        var collection = PagedCollection<Room>.Create<RoomsResponse>(
            Link.ToCollection(nameof(GetAllRooms)),
            rooms.Items.ToArray(),
            rooms.TotalSize,
            pagingOptions);
        collection.Openings = Link.ToCollection(nameof(GetAllRoomOpenings));
        collection.RoomsQuery = FormMetadata.FromResource<Room>(
            Link.ToForm(
                nameof(GetAllRooms),
                null,
                Link.GetMethod,
                Form.QueryRelation));

        return collection;
    }


    [HttpGet("openings", Name = nameof(GetAllRoomOpenings))]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ResponseCache(Duration = 30,
        VaryByQueryKeys = new[] { "offset", "limit", "orderBy", "search" })]
    public async Task<ActionResult<Collection<Opening>>> GetAllRoomOpenings(
        [FromQuery] PagingOptions pagingOptions,
        [FromQuery] SortOptions<Opening, OpeningEntity> sortOptions)
    {
        pagingOptions.Offset ??= _defaultPagingOptions.Offset;
        pagingOptions.Limit ??= _defaultPagingOptions.Limit;

        var openings = await _openingService.GetOpeningsAsync(pagingOptions, sortOptions);

        var collection = PagedCollection<Opening>.Create(
            Link.ToCollection(nameof(GetAllRoomOpenings)),
            openings.Items.ToArray(),
            openings.TotalSize,
            pagingOptions);

        return collection;
    }

    [HttpGet("{roomId}", Name = nameof(GetRoomById))]
    [ProducesResponseType(404)]
    [ProducesResponseType(200)]
    public async Task<ActionResult<Room>> GetRoomById(Guid roomId)
    {
        var room = await _roomService.GetRoomAsync(roomId);
        if (room == null)
        {
            return NotFound();
        }

        return room;
    }

    [HttpPost("{roomId}/bookings", Name = nameof(CreateBookingForRoomAsync))]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    [ProducesResponseType(201)]
    public async Task<ActionResult> CreateBookingForRoomAsync(
        Guid roomId,
        [FromBody] BookingForm bookingForm)
    {
        var room = await _roomService.GetRoomAsync(roomId);

        if (room == null)
        {
            return NotFound();
        }

        var minimumStay = _dateLogicService.GetMinimumStay();
        bool tooShort = (bookingForm.EndAt.Value
            - bookingForm.StartAt.Value)
            < minimumStay;

        if (tooShort)
        {
            return BadRequest(new ApiError(
                $"The minimum booking duration is {minimumStay.TotalHours} hours."));
        }

        var conflictedSlots = await _openingService.GetConflictingSlots(
            roomId, bookingForm.StartAt.Value, bookingForm.EndAt.Value);

        if (conflictedSlots.Any())
        {
            return BadRequest(new ApiError(
                "This time conflicts with an existing booking"));
        }

        var userId = Guid.NewGuid();

        var bookingId = await _bookingService.CreateBookingAsync(
            userId, roomId, bookingForm.StartAt.Value, bookingForm.EndAt.Value);

        return Created(
            Url.Link(nameof(BookingsController.GetBookingById),
            new { bookingId }),
            null);
    }
}