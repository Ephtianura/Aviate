using Aviate.Application.Dto.Airport;
using FluentValidation;

namespace Aviate.Application.Validation.AirportValidator
{
    public class AirportCreateValidator : AbstractValidator<AirportCreateDto>
    {
        public AirportCreateValidator()
        {
            RuleFor(a => a.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100).WithMessage("Airport name cannot exceed 100 characters");

            RuleFor(a => a.Code)
                .NotEmpty().WithMessage("Code is required")
                .Length(3)
                .Matches("^[A-Z]{3}$")
                .WithMessage("The airport code must contain 3 capital letters A-Z");

            RuleFor(a => a.Country)
                .NotEmpty().WithMessage("Country is required")
                .MaximumLength(100).WithMessage("Country cannot exceed 100 characters.");

            RuleFor(a => a.City)
                .NotEmpty().WithMessage("City is required")
                .MaximumLength(100)
                .WithMessage("City cannot exceed 100 characters");
        }
    }
}