using Aviate.Application.Dto;

namespace Aviate.Application.Contracts
{
    public interface IAuthService
    {
        Task<string> Login(string email, string password);
        Task RegisterAsync(UserCreateDto dto);
    }
}