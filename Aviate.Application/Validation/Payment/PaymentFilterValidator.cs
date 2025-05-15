using Aviate.Core.Enums;
using Aviate.Core.Filters;
using FluentValidation;
using System;
using System.Linq;

public class PaymentFilterValidator : AbstractValidator<PaymentFilter>
{
    public PaymentFilterValidator()
    {
        // BookingId може бути null — перевірка не потрібна

        // Статус — якщо вказано, має бути коректним
        RuleFor(f => f.Status)
            .Must(s => !s.HasValue || Enum.IsDefined(typeof(PaymentStatus), s.Value))
            .WithMessage("Invalid payment status");

        // Метод оплати — якщо вказано, має бути коректним
        RuleFor(f => f.Method)
            .Must(m => !m.HasValue || Enum.IsDefined(typeof(PaymentMethod), m.Value))
            .WithMessage("Invalid payment method");

        // Мінімальна сума
        RuleFor(f => f.MinAmount)
            .GreaterThanOrEqualTo(0)
            .When(f => f.MinAmount.HasValue)
            .WithMessage("MinAmount must be >= 0");

        // Максимальна сума
        RuleFor(f => f.MaxAmount)
            .GreaterThanOrEqualTo(0)
            .When(f => f.MaxAmount.HasValue)
            .WithMessage("MaxAmount must be >= 0");

        // Мінімальна сума не може бути більше максимальної
        RuleFor(f => f)
            .Must(f => !f.MinAmount.HasValue || !f.MaxAmount.HasValue || f.MinAmount.Value <= f.MaxAmount.Value)
            .WithMessage("MinAmount cannot be greater than MaxAmount");

        // Діапазон дат створення
        RuleFor(f => f)
            .Must(f => !f.CreatedFrom.HasValue || !f.CreatedTo.HasValue || f.CreatedFrom.Value <= f.CreatedTo.Value)
            .WithMessage("CreatedFrom cannot be later than CreatedTo");

        // Сортування — лише дозволені поля
        RuleFor(f => f.SortBy)
            .Must(s => string.IsNullOrEmpty(s) ||
                        new[] { "CreatedAt", "Amount", "Status", "Method" }
                            .Contains(s, StringComparer.OrdinalIgnoreCase))
            .WithMessage("SortBy must be one of: CreatedAt, Amount, Status, Method");

        // Номер сторінки > 0
        RuleFor(f => f.Page)
            .GreaterThan(0)
            .WithMessage("Page must be greater than 0");

        // Розмір сторінки 1–100
        RuleFor(f => f.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("PageSize must be between 1 and 100");
    }
}
