using Aviate.Core.Contracts;
using Aviate.Core.Filters;
using Aviate.Core.Models;

namespace Aviate.DataAccess.Repositories
{
    public interface IFlightRepository
    {
        Task AddAsync(Flight flight);
        Task DeleteAsync(Flight flight);
        Task<Flight?> GetByFlightNumberAsync(string flightNumber);
        Task<Flight> GetByFlightNumberOrThrowAsync(string flightNumber);
        Task<Flight?> GetByIdAsync(Guid id);
        Task<Flight> GetByIdOrThrowAsync(Guid id);
        Task<PagedResult<Flight>> GetFilteredAsync(FlightFilter filter);
        Task UpdateAsync(Flight flight);
    }
}