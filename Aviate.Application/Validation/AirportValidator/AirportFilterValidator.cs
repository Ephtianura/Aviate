using Aviate.Core.Filters;
using FluentValidation;

namespace Aviate.Application.Validation.AirportValidator
{
    public class AirportFilterValidator : AbstractValidator<AirportFilter>
    {
        public AirportFilterValidator()
        {
            // Пошук — якщо задано, мінімум 2 символи
            RuleFor(f => f.Search)
                .MinimumLength(2)
                .When(f => !string.IsNullOrWhiteSpace(f.Search))
                .WithMessage("Пошуковий запит має містити щонайменше 2 символи");

            // Сортування — лише дозволені поля
            RuleFor(f => f.SortBy)
                .Must(s => string.IsNullOrEmpty(s) ||
                           new[] { "Name", "Code", "City", "Country" }
                               .Contains(s, StringComparer.OrdinalIgnoreCase))
                .WithMessage("Поле сортування має бути одним із: Name, Code, City, Country");

            // Номер сторінки > 0
            RuleFor(f => f.Page)
                .GreaterThan(0)
                .WithMessage("Номер сторінки має бути більшим за 0");

            // Розмір сторінки 1–100
            RuleFor(f => f.PageSize)
                .InclusiveBetween(1, 100)
                .WithMessage("Розмір сторінки має бути в межах від 1 до 100");
        }
    }
}
