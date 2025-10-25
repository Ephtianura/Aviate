using Aviate.Application.Dto.Flight;
using Aviate.Core.Filters;
using Aviate.Core.Models;

namespace Aviate.Application.Services
{
    public interface IFlightService
    {
        Task<Flight> CreateAsync(FlightCreateDto request);
        Task DeleteAsync(Guid id);
        Task<Flight> GetByIdAsync(Guid id);
        Task<PagedResult<Flight>> GetFilteredAsync(FlightFilter filter);
        Task UpdateAsync(Guid id, FlightUpdateDto request);
        Task CreateBatchAsync(List<FlightCreateDto> requests);
    }
}