using Aviate.Application.Dto.Airport;
using Aviate.Application.Dto.Booking;
using FluentValidation;

namespace Aviate.Application.Validation.AirportValidator
{
    public class AirportCreateValidator : AbstractValidator<AirportCreateDto>
    {
        public AirportCreateValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.Code).NotEmpty().WithMessage("Code is required");
            RuleFor(x => x.Country).NotEmpty().WithMessage("Country is required");
            RuleFor(x => x.City).NotEmpty().WithMessage("City is required");
        }
    }
}