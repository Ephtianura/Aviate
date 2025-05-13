using Aviate.Core.Enums;

namespace Aviate.Application.Dto.Flight
{
    public record FlightUpdateDto(
        Guid? AirplaneId,
        Guid? DepartureAirportId,
        Guid? ArrivalAirportId,
        decimal? BasePrice,
        FlightStatus? Status,
        DateTimeOffset? DepartureTime,
        DateTimeOffset? ArrivalTime
    );
}
