using Aviate.Core.Enums;

namespace Aviate.Application.Dto.Airplane
{
    public record AirplaneUpdateDto(
        string? Model,
        string? RegistrationNumber,
        int? Capacity,
        DateTimeOffset? ManufactureDate,
        AirplaneStatus? Status = null
    );
}
