using Aviate.Core.Enums;

namespace Aviate.API.Dto.Admin
{
    public record GetAirplaneAdminResponse(
      Guid Id,
      string Model,
      string RegistrationNumber,
      int Capacity,
      int Status,
      DateTimeOffset ManufactureDate
      //DateTimeOffset UpdatedDate
  );
}
