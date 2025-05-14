using Aviate.Application.Services;
using Aviate.Core.Enums;
using Aviate.Core.Filters;
using Aviate.Core.Models;

namespace Aviate.Application.Contracts
{
    public interface IPaymentService
    {
        Task<List<Payment>> GetByBookingIdAsync(Guid bookingId);
        Task<Payment> GetByIdAsync(Guid id);
        Task<PagedResult<Payment>> GetFilteredAsync(PaymentFilter filter);
        Task<PaymentResult> ProcessPaymentAsync(Booking booking, decimal amount, PaymentMethod paymentMethod);
    }
}