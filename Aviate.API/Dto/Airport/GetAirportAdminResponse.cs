namespace Aviate.API.Dto.Airport
{
    public record GetAirportAdminResponse(
        Guid Id,
        string Name,
        string Code,
        string Country,
        string City
  );
}
