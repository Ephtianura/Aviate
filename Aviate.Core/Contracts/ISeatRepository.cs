using Aviate.Core.Filters;
using Aviate.Core.Models;

namespace Aviate.DataAccess.Repositories
{
    public interface ISeatRepository
    {
        Task AddRangeAsync(IEnumerable<Seat> seats);
        Task<List<Seat>> GetByFlightIdAsync(Guid flightId);
        Task<Seat?> GetByIdAsync(Guid id);
        Task<PagedResult<Seat>> GetFilteredAsync(SeatFilter filter);
        Task UpdateAsync(Seat seat);
    }
}