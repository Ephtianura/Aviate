using Aviate.Application.Dto.Airplane;
using FluentValidation;

namespace Aviate.Application.Validation.AirplaneValidator
{
    public class AirplaneCreateValidator : AbstractValidator<AirplaneRequest>
    {
        public AirplaneCreateValidator()
        {
            RuleFor(a => a.Model)
                .NotEmpty().WithMessage("Model is required")
                .MaximumLength(100).WithMessage("Maximum model length is 100 characters");

            RuleFor(a => a.RegistrationNumber)
                .NotEmpty().WithMessage("RegistrationNumber is required")
                .MaximumLength(20).WithMessage("Maximum RegistrationNumber length is 100 characters");

            RuleFor(a => a.Capacity)
                .NotEmpty().WithMessage("Capacity is required")
                .GreaterThan(0)
                .When(a => a.Capacity > 0)
                .WithMessage("Capacity must be greater than 0");

            RuleFor(a => a.ManufactureDate)
                .NotEmpty().WithMessage("ManufactureDate is required")
                .LessThanOrEqualTo(DateTimeOffset.UtcNow)
                .WithMessage("Manufacture date cannot be in the future");
        }
    }
}