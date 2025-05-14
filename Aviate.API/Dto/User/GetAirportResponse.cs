namespace Aviate.API.Dto.User
{
    public record GetAirportResponse(
        Guid Id,
        string Name,
        string Code,
        string Country,
        string City
    );
}
