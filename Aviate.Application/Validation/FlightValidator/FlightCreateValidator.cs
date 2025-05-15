using Aviate.Application.Dto.Flight;
using FluentValidation;

namespace Aviate.Application.Validation.FlightValidator
{
    public class FlightCreateValidator : AbstractValidator<FlightCreateDto>
    {

        public FlightCreateValidator()
        {
            RuleFor(f => f.AirplaneId)
                .NotEmpty()
                .WithMessage("AirplaneId is required");

            RuleFor(f => f.DepartureAirportId)
                .NotEmpty()
                .WithMessage("DepartureAirportId is required");

            RuleFor(f => f.ArrivalAirportId)
                .NotEmpty()
                .WithMessage("ArrivalAirportId is required")
                .NotEqual(f => f.DepartureAirportId)
                .WithMessage("ArrivalAirportId cannot be the same as DepartureAirportId");

            RuleFor(f => f.BasePrice)
                .GreaterThanOrEqualTo(0)
                .WithMessage("BasePrice must be >= 0");

            RuleFor(f => f.DepartureTime)
                .LessThan(f => f.ArrivalTime)
                .WithMessage("DepartureTime must be earlier than ArrivalTime");

            RuleFor(f => f.ArrivalTime)
                .GreaterThan(f => f.DepartureTime)
                .WithMessage("ArrivalTime must be later than DepartureTime");

            RuleFor(f => f.EconomySeats)
                .GreaterThanOrEqualTo(0)
                .WithMessage("EconomySeats must be >= 0");

            RuleFor(f => f.BusinessSeats)
                .GreaterThanOrEqualTo(0)
                .WithMessage("BusinessSeats must be >= 0");

            RuleFor(f => f.FirstClassSeats)
                .GreaterThanOrEqualTo(0)
                .WithMessage("FirstClassSeats must be >= 0");
        }
    }

}
