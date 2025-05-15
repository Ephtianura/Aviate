namespace Aviate.API.Dto.User.Booking
{
    public record GetSeatBookingResponse(
        Guid Id,
        string Class,
        string SeatNumber
    );
}
