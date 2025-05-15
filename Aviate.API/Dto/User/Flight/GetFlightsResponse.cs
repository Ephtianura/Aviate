using Aviate.Core.Enums;

namespace Aviate.API.Dto.User.Flight
{
    public record GetFlightsResponse(
        Guid Id,
        string FlightNumber,
        decimal BasePrice,
        FlightStatus Status,
        DateTimeOffset DepartureTime,
        DateTimeOffset ArrivalTime,
        Guid AirplaneId,
        Guid DepartureAirportId,
        Guid ArrivalAirportId,
        GetAirplaneResponse Airplane,
        GetAirportResponse DepartureAirport,
        GetAirportResponse ArrivalAirport
);
}
