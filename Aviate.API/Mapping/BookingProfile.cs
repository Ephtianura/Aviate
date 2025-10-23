using AutoMapper;
using Aviate.API.Dto;
using Aviate.API.Dto.User;
using Aviate.API.Dto.User.Booking;
using Aviate.API.Dto.User.Flight;
using Aviate.Core.Filters;
using Aviate.Core.Models;

namespace Aviate.API.Mapping
{
    public class BookingProfile : Profile
    {
        public BookingProfile()
        {
            // Вложенные маппинги
            CreateMap<User, GetUserResponse>();
            CreateMap<Flight, GetFlightBookingResponse>()
                //.ForMember(dest => dest.Airplane, opt => opt.MapFrom(src => src.Airplane))
                .ForMember(dest => dest.DepartureAirport, opt => opt.MapFrom(src => src.DepartureAirport))
                .ForMember(dest => dest.ArrivalAirport, opt => opt.MapFrom(src => src.ArrivalAirport));

            CreateMap<Seat, GetSeatBookingResponse>();

            // Маппинг одного бронирования
            CreateMap<Booking, GetBookingResponse>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.Flight, opt => opt.MapFrom(src => src.Flight))
                .ForMember(dest => dest.Seat, opt => opt.MapFrom(src => src.Seat));

            CreateMap<BookingUserFilter, BookingAdminFilter>()
            .ForMember(dest => dest.UserId, opt => opt.Ignore());

            // Маппинг страниц
            CreateMap<PagedResult<Booking>, PagedResultResponse<GetBookingResponse>>()
                .ConvertUsing(new PagedResultConverter<Booking, GetBookingResponse>());
        }
    }
}
