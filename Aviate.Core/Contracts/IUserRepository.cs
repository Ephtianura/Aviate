using Aviate.Core.Filters;
using Aviate.Core.Models;

namespace Aviate.Core.Contracts
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task DeleteAsync(User user);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(Guid id);
        Task<PagedResult<User>> GetFilteredAsync(UserFilter filter);
        Task UpdateAsync(User user);
    }
}