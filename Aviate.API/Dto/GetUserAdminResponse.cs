using System.ComponentModel.DataAnnotations;

namespace Aviate.API.Dto
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
