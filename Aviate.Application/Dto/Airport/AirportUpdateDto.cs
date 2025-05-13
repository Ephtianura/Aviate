namespace Aviate.Application.Dto.Airport
{
    public record AirportUpdateDto(
        string? Name,
        string? Code,
        string? Country,
        string? City
    );
}
