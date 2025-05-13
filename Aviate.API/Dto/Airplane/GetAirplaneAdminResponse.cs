using Aviate.Core.Enums;

namespace Aviate.API.Dto.Airplane
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
