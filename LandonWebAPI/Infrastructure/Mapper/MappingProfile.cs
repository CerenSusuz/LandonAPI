using AutoMapper;
using LandonWebAPI.Models.DbEntities;
using LandonWebAPI.Models.DTOs;

namespace LandonWebAPI.Infrastructure.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RoomEntity, Room>()
                .ForMember(dest => dest.Rate, opt => opt.MapFrom(src => src.Rate / 100.0m))
                .ForMember(dest => dest.Self, opt => opt.MapFrom(src =>
                    Link.To(
                        nameof(Controllers.RoomsController.GetRoomById),
                        new { roomId = src.Id })));

        CreateMap<OpeningEntity, Opening>()
            .ForMember(dest => dest.Rate, opt => opt.MapFrom(src => src.Rate / 100m))
            .ForMember(dest => dest.StartAt, opt => opt.MapFrom(src => src.StartAt))
            .ForMember(dest => dest.EndAt, opt => opt.MapFrom(src => src.EndAt))
            .ForMember(dest => dest.Room, opt => opt.MapFrom(src =>
                Link.To(
                    nameof(Controllers.RoomsController.GetRoomById),
                    new { roomId = src.RoomId })));

        CreateMap<BookingEntity, Booking>()
            .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.Total / 100m))
            .ForMember(dest => dest.Self, opt => opt.MapFrom(src =>
                Link.To(
                    nameof(Controllers.BookingsController.GetBookingById),
                    new { bookingId = src.Id })))
            .ForMember(dest => dest.Room, opt => opt.MapFrom(src =>
                Link.To(
                    nameof(Controllers.RoomsController.GetRoomById),
                    new { roomId = src.Id })));
    }
}