using Aviate.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Aviate.Application.Dto
{
    public record AirplaneRequest(
        string Model,
        string RegistrationNumber,
        int Capacity,
        DateTimeOffset ManufactureDate,
        AirplaneStatus? Status = null
    );
}
