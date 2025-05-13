using Aviate.Core.Filters;
using Aviate.Core.Enums;
using FluentValidation;
using System;
using System.Linq;

namespace Aviate.Application.Validation
{
    public class AirplaneFilterValidator : AbstractValidator<AirplaneFilter>
    {
        public AirplaneFilterValidator()
        {
            // Пошук — якщо задано, мінімум 2 символи
            RuleFor(f => f.Search)
                .MinimumLength(2)
                .When(f => !string.IsNullOrWhiteSpace(f.Search))
                .WithMessage("Пошуковий запит має містити щонайменше 2 символи");

            // Місткість — min ≤ max
            RuleFor(f => f)
                .Must(f => !(f.MinCapacity.HasValue && f.MaxCapacity.HasValue && f.MinCapacity > f.MaxCapacity))
                .WithMessage("Мінімальна місткість не може бути більшою за максимальну");

            // Дата виробництва — From ≤ To
            RuleFor(f => f)
                .Must(f => !(f.ManufactureFrom.HasValue && f.ManufactureTo.HasValue && f.ManufactureFrom > f.ManufactureTo))
                .WithMessage("'Дата виробництва від' не може бути пізніше, ніж 'Дата до'");

            // Сортування — лише дозволені поля
            RuleFor(f => f.SortBy)
                .Must(s => string.IsNullOrEmpty(s) ||
                           new[] { "Model", "RegistrationNumber", "Capacity", "ManufactureDate", "Status" }
                               .Contains(s, StringComparer.OrdinalIgnoreCase))
                .WithMessage("Поле сортування має бути одним із: Model, RegistrationNumber, Capacity, ManufactureDate, Status");

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
