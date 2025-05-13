using Aviate.Core.Filters;
using FluentValidation;

namespace Aviate.Application.Validation.FlightValidator
{
    public class FlightFilterValidator : AbstractValidator<FlightFilter>
    {
        public FlightFilterValidator()
        {
            RuleFor(f => f.Search)
                .MinimumLength(2)
                .When(f => !string.IsNullOrWhiteSpace(f.Search))
                .WithMessage("Search term must be at least 2 characters long");

            RuleFor(f => f)
                .Must(f => !(f.DepartureFrom.HasValue && f.DepartureTo.HasValue && f.DepartureFrom > f.DepartureTo))
                .WithMessage("'Departure from' cannot be later than 'Departure to'");

            RuleFor(f => f)
                .Must(f => !(f.ArrivalFrom.HasValue && f.ArrivalTo.HasValue && f.ArrivalFrom > f.ArrivalTo))
                .WithMessage("'Arrival from' cannot be later than 'Arrival to'");

            RuleFor(f => f.SortBy)
                .Must(s => string.IsNullOrEmpty(s) ||
                           new[] { "FlightNumber", "DepartureTime", "ArrivalTime", "BasePrice", "Status" }
                               .Contains(s, StringComparer.OrdinalIgnoreCase))
                .WithMessage("SortBy must be one of: FlightNumber, DepartureTime, ArrivalTime, BasePrice, Status");

            RuleFor(f => f.Page)
                .GreaterThan(0)
                .WithMessage("Page number must be greater than 0");

            RuleFor(f => f.PageSize)
                .InclusiveBetween(1, 100)
                .WithMessage("PageSize must be between 1 and 100");
        }
    }
}
