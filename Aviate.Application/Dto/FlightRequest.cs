using Aviate.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Aviate.Application.Dto
{
    public record FlightRequest(
        [Required] string FlightNumber,
        [Required] Guid AirplaneId,
        [Required] Guid DepartureAirportId,
        [Required] Guid ArrivalAirportId,
        [Required] decimal BasePrice,
        [Required] DateTimeOffset DepartureTime,
        [Required] DateTimeOffset ArrivalTime,
        int EconomySeats,
        int BusinessSeats,
        int FirstClassSeats,
        FlightStatus? Status = null
    );
}