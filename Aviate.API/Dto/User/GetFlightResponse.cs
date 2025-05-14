using Aviate.Core.Enums;
using Aviate.Core.Models;

namespace Aviate.API.Dto.User
{
    record GetFlightResponse(
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
        GetAirportResponse ArrivalAirport,
        List<GetSeatResponse> Seats
 );
}
