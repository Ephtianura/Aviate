namespace Aviate.API.Dto.User.Booking
{
    public record BookingCreateRequest
    (
        Guid FlightId, Guid SeatId
    );
}
