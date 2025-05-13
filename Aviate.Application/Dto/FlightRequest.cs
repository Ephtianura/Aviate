using Aviate.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Aviate.Application.Dto
{
    public record FlightRequest(
        string FlightNumber,
        Guid AirplaneId,
        Guid DepartureAirportId,
        Guid ArrivalAirportId,
        decimal BasePrice,
        DateTimeOffset DepartureTime,
        DateTimeOffset ArrivalTime,
        int EconomySeats,
        int BusinessSeats,
        int FirstClassSeats,
        FlightStatus? Status = null
    );
}