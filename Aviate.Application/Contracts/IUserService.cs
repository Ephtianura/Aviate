using Aviate.Application.Dto;
using Aviate.Core.Contracts;
using Aviate.Core.Filters;
using Aviate.Core.Models;

namespace Aviate.Application.Contracts
{
    public interface IUserService
    {
        Task DeleteAsync(Guid id);
        Task<User> GetByIdAsync(Guid id);
        Task<PagedResult<User>> GetFilteredAsync(UserFilter filter);
        Task UpdateProfileAsync(Guid id, UserUpdateDto dto);
    }
}