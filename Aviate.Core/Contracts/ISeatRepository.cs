using Aviate.Core.Contracts;
using Aviate.Core.Filters;
using Aviate.Core.Models;

namespace Aviate.DataAccess.Repositories
{
    public interface ISeatRepository
    {
        Task AddAsync(Seat seat);
        Task DeleteAsync(Seat seat);
        Task<Seat?> GetByIdAsync(Guid id);
        Task<PagedResult<Seat>> GetFilteredAsync(SeatFilter filter);
        Task UpdateAsync(Seat seat);
    }
}