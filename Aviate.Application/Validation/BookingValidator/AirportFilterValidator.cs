using Aviate.Core.Enums;
using Aviate.Core.Filters;
using FluentValidation;

namespace Aviate.Application.Validation.BookingValidator
{
    public class BookingFilterValidator : AbstractValidator<BookingAdminFilter>
    {
        public BookingFilterValidator()
        {
            // UserId и FlightId могут быть null — проверка не требуется

            // Статус — должен быть определён в BookingStatus, если указан
            RuleFor(f => f.Status)
                .Must(s => !s.HasValue || Enum.IsDefined(typeof(BookingStatus), s.Value))
                .WithMessage("Invalid booking status");

            // Проверка минимальной и максимальной цены
            RuleFor(f => f.MinTotalPrice)
                .GreaterThanOrEqualTo(0)
                .When(f => f.MinTotalPrice.HasValue)
                .WithMessage("MinTotalPrice must be >= 0");

            RuleFor(f => f.MaxTotalPrice)
                .GreaterThanOrEqualTo(0)
                .When(f => f.MaxTotalPrice.HasValue)
                .WithMessage("MaxTotalPrice must be >= 0");

            // Если заданы обе цены — минимальная ≤ максимальной
            RuleFor(f => f)
                .Must(f => !f.MinTotalPrice.HasValue || !f.MaxTotalPrice.HasValue || f.MinTotalPrice.Value <= f.MaxTotalPrice.Value)
                .WithMessage("MinTotalPrice cannot be greater than MaxTotalPrice");

            // Проверка диапазона дат
            RuleFor(f => f)
                .Must(f => !f.BookingFrom.HasValue || !f.BookingTo.HasValue || f.BookingFrom.Value <= f.BookingTo.Value)
                .WithMessage("BookingFrom cannot be later than BookingTo");

            // Сортування — лише дозволені поля
            RuleFor(f => f.SortBy)
                .Must(s => string.IsNullOrEmpty(s) ||
                            new[] { "BookingDate", "TotalPrice", "Status" }
                                .Contains(s, StringComparer.OrdinalIgnoreCase))
                .WithMessage("SortBy must be one of: BookingDate, TotalPrice, Status");

            // Номер сторінки > 0
            RuleFor(f => f.Page)
                .GreaterThan(0)
                .WithMessage("Page number must be greater than 0");

            // Розмір сторінки 1–100
            RuleFor(f => f.PageSize)
                .InclusiveBetween(1, 100)
                .WithMessage("PageSize must be between 1 and 100");
        }

    }
}
