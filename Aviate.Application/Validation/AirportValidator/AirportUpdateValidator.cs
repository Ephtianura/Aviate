using Aviate.Application.Dto.Airport;
using FluentValidation;

namespace Aviate.Application.Validation.AirportValidator
{
    public class AirportUpdateValidator : AbstractValidator<AirportUpdateDto>
    {
        public AirportUpdateValidator()
        {
            RuleFor(a => a.Name)
                .MaximumLength(100)
                .When(a => !string.IsNullOrEmpty(a.Name))
                .WithMessage("Airport name cannot exceed 100 characters");

            RuleFor(a => a.Code)
                .Length(3)
                .Matches("^[A-Z]{3}$")
                .When(a => !string.IsNullOrEmpty(a.Code))
                .WithMessage("The airport code must contain 3 capital letters A-Z");

            RuleFor(a => a.Country)
                .MaximumLength(100)
                .When(a => !string.IsNullOrEmpty(a.Country))
                .WithMessage("Country cannot exceed 100 characters.");

            RuleFor(a => a.City)
                .MaximumLength(100)
                .When(a => !string.IsNullOrEmpty(a.City))
                .WithMessage("City cannot exceed 100 characters");
        }
    }
}