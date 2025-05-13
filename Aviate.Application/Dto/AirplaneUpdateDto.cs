using Aviate.Core.Enums;

namespace Aviate.Application.Dto
{
    public record AirplaneUpdateDto(
        string? Model,
        string? RegistrationNumber,
        int? Capacity,
        DateTimeOffset? ManufactureDate,
        AirplaneStatus? Status = null
    );
}
