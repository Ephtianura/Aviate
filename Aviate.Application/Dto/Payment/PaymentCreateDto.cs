using Aviate.Core.Enums;

namespace Aviate.Application.Dto.Payment
{
    public record PaymentCreateDto
    (
        Guid BookingId,
        decimal Amount,
        PaymentMethod Method
    );
}