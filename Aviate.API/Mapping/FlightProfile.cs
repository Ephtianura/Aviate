using AutoMapper;
using Aviate.API.Dto;
using Aviate.API.Dto.User;
using Aviate.API.Dto.Admin;
using Aviate.API.Dto.User.Booking;
using Aviate.API.Dto.User.Flight;
using Aviate.API.Mapping;
using Aviate.Core.Filters;
using Aviate.Core.Models;

public class FlightProfile : Profile
{
    public FlightProfile()
    {
        // Мапінг вкладених об'єктів
        CreateMap<Airplane, GetAirplaneResponse>().ReverseMap();
        CreateMap<Airport, GetAirportResponse>().ReverseMap();
        CreateMap<Seat, GetSeatAdminResponse>().ReverseMap();

        // Для списка рейсів (менше інфи)
        CreateMap<Flight, GetFlightsResponse>()
            .ForMember(dest => dest.Airplane, opt => opt.MapFrom(src => src.Airplane))
            .ForMember(dest => dest.DepartureAirport, opt => opt.MapFrom(src => src.DepartureAirport))
            .ForMember(dest => dest.ArrivalAirport, opt => opt.MapFrom(src => src.ArrivalAirport)); 

        // Для повного рейса
        CreateMap<Flight, GetFlightResponse>()
            .ForMember(dest => dest.Airplane, opt => opt.MapFrom(src => src.Airplane))
            .ForMember(dest => dest.DepartureAirport, opt => opt.MapFrom(src => src.DepartureAirport))
            .ForMember(dest => dest.ArrivalAirport, opt => opt.MapFrom(src => src.ArrivalAirport))
            .ForMember(dest => dest.Seats, opt => opt.MapFrom(src => src.Seats));

        CreateMap<PagedResult<Flight>, PagedResultResponse<GetFlightsResponse>>()
                .ConvertUsing(new PagedResultConverter<Flight, GetFlightsResponse>());

        CreateMap<PagedResult<Flight>, PagedResultResponse<GetFlightResponse>>()
                .ConvertUsing(new PagedResultConverter<Flight, GetFlightResponse>());

        CreateMap<PagedResult<Seat>, PagedResultResponse<GetSeatAdminResponse>>()
                .ConvertUsing(new PagedResultConverter<Seat, GetSeatAdminResponse>());


    }
}
