using Aviate.Application.Dto.Airplane;
using Aviate.Core.Enums;
using FluentValidation;

namespace Aviate.Application.Validation.AirplaneValidator
{
    public class AirplaneUpdateValidator : AbstractValidator<AirplaneUpdateDto>
    {
        public AirplaneUpdateValidator()
        {
            RuleFor(a => a.Model)
                .MaximumLength(100)
                .When(a => !string.IsNullOrEmpty(a.Model))
                .WithMessage("Maximum model length is 100 characters");

            RuleFor(a => a.RegistrationNumber)
                .MaximumLength(20)
                .When(a => !string.IsNullOrEmpty(a.RegistrationNumber))
                .WithMessage("Maximum RegistrationNumber length is 100 characters");

            RuleFor(a => a.Capacity)
                .GreaterThan(0)
                .When(a => a.Capacity > 0)
                .WithMessage("Capacity must be greater than 0");

            RuleFor(a => a.ManufactureDate)
                .LessThanOrEqualTo(DateTimeOffset.UtcNow)
                .When(a => a.ManufactureDate != default)
                .WithMessage("Manufacture date cannot be in the future");

            RuleFor(a => a.Status)
                .Must(s => !s.HasValue || Enum.IsDefined(typeof(AirplaneStatus), s.Value))
                .WithMessage("Invalid airplane status");
        }
    }
}