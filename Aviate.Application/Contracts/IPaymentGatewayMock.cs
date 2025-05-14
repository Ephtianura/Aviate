using Aviate.Application.Services;
using Aviate.Core.Enums;

namespace Aviate.Infrastructure.Payment
{
    public interface IPaymentGatewayMock
    {
        Task<PaymentResult> ProcessPaymentAsync(Guid bookingId, decimal amount, PaymentMethod method);
    }
}