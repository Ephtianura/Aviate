using Aviate.Core.Enums;
using Aviate.Core.Models;

namespace Aviate.API.Dto.User
{
    record GetFlightResponse(
        Guid Id,
        Guid AirplaneId,
        Guid DepartureAirportId,
        Guid ArrivalAirportId,

        string FlightNumber,
        decimal BasePrice,

        FlightStatus Status,

        DateTimeOffset DepartureTime,
        DateTimeOffset ArrivalTime,

        Airplane Airplane,
        Airport DepartureAirport,
        Airport ArrivalAirport,

        IReadOnlyCollection<Seat> Seats
 );
}
