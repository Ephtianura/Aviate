using Aviate.Application.Services;
using Aviate.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aviate.Infrastructure.Payment
{
    public class PaymentGatewayMock : IPaymentGatewayMock
    {
        public async Task<PaymentResult> ProcessPaymentAsync(Guid bookingId, decimal amount, PaymentMethod method)
        {
            // Імітація оплати
            await Task.Delay(1000);

            var success = Random.Shared.Next(0, 100) > 5; // 95% успіху

            return success
                ? new PaymentResult(true, Guid.NewGuid().ToString(), null)
                : new PaymentResult(false, null, "Payment declined by payment service");
        }
    }
}
