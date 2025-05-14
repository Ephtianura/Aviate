using Aviate.Core.Enums;

namespace Aviate.API.Dto.User
{
    public record GetAirplaneResponse(
    Guid Id,
    string Model,
    string RegistrationNumber,
    int Capacity,
    AirplaneStatus Status,
    DateTimeOffset ManufactureDate
);
}
