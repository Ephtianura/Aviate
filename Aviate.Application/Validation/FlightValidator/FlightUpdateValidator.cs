using Aviate.Application.Dto.Flight;
using FluentValidation;

namespace Aviate.Application.Validation.FlightValidator
{
    public class FlightUpdateValidator : AbstractValidator<FlightUpdateDto>
    {

        public FlightUpdateValidator()
        {
            RuleFor(f => f.AirplaneId)
                .NotEmpty()
                .When(f => f.AirplaneId.HasValue)
                .WithMessage("AirplaneId cannot be empty");

            RuleFor(f => f.DepartureAirportId)
                .NotEmpty()
                .When(f => f.DepartureAirportId.HasValue)
                .WithMessage("DepartureAirportId cannot be empty");

            RuleFor(f => f.ArrivalAirportId)
                .NotEmpty()
                .When(f => f.ArrivalAirportId.HasValue)
                .WithMessage("ArrivalAirportId cannot be empty");

            RuleFor(f => f.BasePrice)
                .GreaterThanOrEqualTo(0)
                .When(f => f.BasePrice.HasValue)
                .WithMessage("BasePrice must be >= 0");

            RuleFor(f => f.DepartureTime)
                .LessThan(f => f.ArrivalTime.Value)
                .When(f => f.DepartureTime.HasValue && f.ArrivalTime.HasValue)
                .WithMessage("DepartureTime must be earlier than ArrivalTime");

            RuleFor(f => f.Status)
                .IsInEnum()
                .When(f => f.Status.HasValue)
                .WithMessage("Invalid flight status");
        }


    }
}