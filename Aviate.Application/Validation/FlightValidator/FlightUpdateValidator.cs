using Aviate.Application.Dto.Flight;
using FluentValidation;

namespace Aviate.Application.Validation.FlightValidator
{
    public class FlightUpdateValidator : AbstractValidator<FlightUpdateDto>
    {

        public FlightUpdateValidator()
        {
            RuleFor(f => f.BasePrice)
                .NotEmpty().WithMessage("NotImplementedException")
                .GreaterThanOrEqualTo(0)
                .WithMessage("NotImplementedException");

            
            RuleFor(f => f.DepartureTime)
                .NotEmpty().WithMessage("NotImplementedException")
                .LessThan(f => f.ArrivalTime)
                .WithMessage("Departure time must be earlier than arrival time");

            //RuleFor(f => f.Status)
            //    .Must(s => !s.HasValue || Enum.IsDefined(typeof(FlightStatus), s.Value))
            //    .WithMessage("Invalid flight status");
        }
    }
}