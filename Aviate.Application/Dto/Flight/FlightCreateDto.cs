using Aviate.Core.Models;

namespace Aviate.Application.Dto.Flight
{
    public record BookingCreateDto(
            Guid AirplaneId,
            Guid DepartureAirportId,
            Guid ArrivalAirportId,
            decimal BasePrice,
            DateTimeOffset DepartureTime,
            DateTimeOffset ArrivalTime,
            int EconomySeats,
            int BusinessSeats,
            int FirstClassSeats
    );
}
