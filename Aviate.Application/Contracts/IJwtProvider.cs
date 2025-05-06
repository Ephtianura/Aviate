using Aviate.Core.Models;

namespace Aviate.Application.Contracts
{
    public interface IJwtProvider
    {
        string GenerateToken(User user);
    }
}