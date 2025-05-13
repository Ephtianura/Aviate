namespace Aviate.Application.Dto.Airport
{
    public record AirportCreateDto(
        string Name,
        string Code,
        string Country,
        string City
    );
}
