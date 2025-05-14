namespace Aviate.API.Dto.User
{
    public record GetSeatResponse(
        Guid Id,
        string Class,
        string SeatNumber,
        bool IsBooked
    );
}
