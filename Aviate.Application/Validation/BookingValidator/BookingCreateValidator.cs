using Aviate.Application.Dto.Booking;
using FluentValidation;

namespace Aviate.Application.Validation.BookingValidator
{
    public class BookingCreateValidator : AbstractValidator<BookingCreateDto>
    {
        public BookingCreateValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is required");
            RuleFor(x => x.FlightId).NotEmpty().WithMessage("FlightId is required");
            RuleFor(x => x.SeatId).NotEmpty().WithMessage("SeatId is required");
        }
    }
}