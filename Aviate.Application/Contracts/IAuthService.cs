using Aviate.Application.Dto;

namespace Aviate.Application.Contracts
{
    public interface IAuthService
    {
        Task<string> Login(LoginUserRequest request);
        Task RegisterAsync(RegisterUserRequest request);
    }
}