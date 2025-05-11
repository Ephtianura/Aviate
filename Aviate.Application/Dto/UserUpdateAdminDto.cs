using Aviate.Core.Models;

namespace Aviate.Application.Dto
{
    public class UserUpdateAdminDto
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public UserRole? Role { get; set; } 
    }
}
