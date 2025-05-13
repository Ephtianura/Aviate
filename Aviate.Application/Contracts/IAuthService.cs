using Aviate.Application.Dto.User;

namespace Aviate.Application.Contracts
{
    public interface IAuthService
    {
        Task<string> Login(LoginUserRequest request);
        Task RegisterAsync(RegisterUserRequest request);
    }
}