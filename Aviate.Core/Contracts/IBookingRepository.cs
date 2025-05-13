using Aviate.Core.Contracts;
using Aviate.Core.Filters;
using Aviate.Core.Models;

namespace Aviate.DataAccess.Repositories
{
    public interface IBookingRepository
    {
        Task AddAsync(Booking booking);
        Task DeleteAsync(Booking booking);
        Task<Booking?> GetByIdAsync(Guid id);
        Task<PagedResult<Booking>> GetFilteredAsync(BookingFilter filter);
        Task UpdateAsync(Booking booking);
    }
}