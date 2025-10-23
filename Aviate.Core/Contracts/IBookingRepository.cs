using Aviate.Core.Filters;
using Aviate.Core.Models;

namespace Aviate.DataAccess.Repositories
{
    public interface IBookingRepository
    {
        Task AddAsync(Booking booking);
        Task DeleteAsync(Booking booking);
        Task<Booking?> GetByIdAsync(Guid id);
        Task<List<Booking>> GetByUserIdAsync(Guid userId);
        Task<PagedResult<Booking>> GetFilteredAsync(BookingAdminFilter filter);
        Task UpdateAsync(Booking booking);
    }
}