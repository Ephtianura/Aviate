using Aviate.Core.Filters;
using Aviate.Core.Models;

namespace Aviate.Application.Contracts
{
    public interface ISeatService
    {
        Task<PagedResult<Seat>> GetFilteredAsync(SeatFilter filter);
        Task<List<Seat>> GetSeatsByFlightAsync(Guid flightId);
    }
}