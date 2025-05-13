using Aviate.Application.Dto;
using Aviate.Core.Enums;
using FluentValidation;

namespace Aviate.Application.Validation.FlightValidator
{
    public class FlightRequestValidator : AbstractValidator<FlightRequest>
    {
        public FlightRequestValidator()
        {
            RuleFor(f => f.FlightNumber)
                .NotEmpty()
                .MaximumLength(20);

            RuleFor(f => f.BasePrice)
                .GreaterThanOrEqualTo(0);

            RuleFor(f => f.DepartureTime)
                .LessThan(f => f.ArrivalTime)
                .WithMessage("Departure time must be earlier than arrival time");

            RuleFor(f => f.Status)
                .Must(s => !s.HasValue || Enum.IsDefined(typeof(FlightStatus), s.Value))
                .WithMessage("Invalid flight status");
        }
    }
}