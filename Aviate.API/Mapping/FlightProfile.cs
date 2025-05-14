using AutoMapper;
using Aviate.API.Dto;
using Aviate.API.Dto.User;
using Aviate.API.Mapping;
using Aviate.Core.Filters;
using Aviate.Core.Models;

public class FlightProfile : Profile
{
    public FlightProfile()
    {
        // Мапінг вкладених об'єктів
        CreateMap<Airplane, GetAirplaneResponse>();
        CreateMap<Airport, GetAirportResponse>();
        CreateMap<Seat, GetSeatResponse>();

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

        CreateMap<PagedResult<Seat>, PagedResultResponse<GetSeatResponse>>()
                .ConvertUsing(new PagedResultConverter<Seat, GetSeatResponse>());
    }
}
