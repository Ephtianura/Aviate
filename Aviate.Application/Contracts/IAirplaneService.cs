using Aviate.Application.Dto;
using Aviate.Core.Contracts;
using Aviate.Core.Filters;
using Aviate.Core.Models;

namespace Aviate.Application.Contracts
{
    public interface IAirplaneService
    {
        Task<Airplane> CreateAsync(AirplaneRequest dto);
        Task DeleteAsync(Guid id);
        Task<Airplane> GetByIdAsync(Guid id);
        Task<PagedResult<Airplane>> GetFilteredAsync(AirplaneFilter filter);
        Task UpdateAsync(Guid id, AirplaneUpdateDto dto);
    }
}