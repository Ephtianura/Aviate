namespace Aviate.API.Dto.Admin
{
    public record GetSeatAdminResponse(
        Guid Id,
        Guid FlightId,
        string Class,
        string SeatNumber,
        bool IsBooked
    );
}
