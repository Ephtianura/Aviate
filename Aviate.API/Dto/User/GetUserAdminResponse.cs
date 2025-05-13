using System.ComponentModel.DataAnnotations;

namespace Aviate.API.Dto.User
{
    public record GetUserAdminResponse(
      Guid Id,
      string FullName,
      string Email,
      string? Phone,
      int Role,
      DateTimeOffset RegistrationDate,
      DateTimeOffset UpdatedDate
  );
}
