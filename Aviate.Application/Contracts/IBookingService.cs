using Aviate.Application.Dto.Booking;
using Aviate.Application.Dto.Payment;
using Aviate.Core.Enums;
using Aviate.Core.Filters;
using Aviate.Core.Models;

namespace Aviate.Application.Contracts
{
    public interface IBookingService
    {
        Task<Booking> GetByIdAsync(Guid id);
        Task<Booking> CreateAsync(BookingCreateDto request);
        Task<List<Booking>> GetByUserIdAsync(Guid userId);
        Task<PagedResult<Booking>> GetFilteredAsync(BookingFilter filter);
        Task<PaymentResult> PayBookingAsync(Guid userId, Guid bookingId, PaymentMethod paymentMethod);
        Task CancelBookingAsync(Guid userId, Guid bookingId);
        Task DeleteAsync(Guid id);

    }
}