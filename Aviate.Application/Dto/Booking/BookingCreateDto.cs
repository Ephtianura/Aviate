using Aviate.Core.Enums;
using Aviate.Core.Models;

namespace Aviate.Application.Dto.Booking
{
    public record BookingCreateDto(
        Guid UserId,
        Guid FlightId,
        Guid? SeatId
   );
}
