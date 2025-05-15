using Aviate.Core.Enums;

namespace Aviate.API.Dto.User.Booking
{
     record GetBookingResponse
    (
         Guid Id,
         Guid UserId,
         Guid FlightId,
         Guid SeatId,

         decimal TotalPrice,
         BookingStatus Status,
         DateTimeOffset BookingDate,

         GetUserResponse User, 
         GetFlightBookingResponse Flight, 
         GetSeatBookingResponse Seat
    );
}