namespace Aviate.API.Dto.Admin
{
    public record GetAirportAdminResponse(
        Guid Id,
        string Name,
        string Code,
        string Country,
        string City
  );
}
