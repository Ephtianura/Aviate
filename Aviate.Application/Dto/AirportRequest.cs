using System.ComponentModel.DataAnnotations;

namespace Aviate.Application.Dto
{
    public record AirportRequest(
        [Required] string Name,
        [Required] string Code,
        [Required] string Country,
        [Required] string City
    );
}
