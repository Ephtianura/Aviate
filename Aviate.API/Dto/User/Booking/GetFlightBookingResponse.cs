using Aviate.Core.Enums;

namespace Aviate.API.Dto.User.Booking
{
    record GetFlightBookingResponse(
        Guid Id,
        string FlightNumber,
        FlightStatus Status,
        DateTimeOffset DepartureTime,
        DateTimeOffset ArrivalTime,
        //GetAirplaneResponse Airplane,
        GetAirportResponse DepartureAirport,
        GetAirportResponse ArrivalAirport
 );
}
