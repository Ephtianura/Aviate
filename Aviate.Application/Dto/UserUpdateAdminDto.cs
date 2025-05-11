using Aviate.Core.Enums;

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
