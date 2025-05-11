using System.Net.Mail;

namespace Aviate.Core.Models
{
    // ================= USER =================
    public class User
    {
        private User(string fullName, string email, string passwordHash)
        {
            Id = Guid.NewGuid();
            FullName = fullName.Trim();
            Email = email;
            PasswordHash = passwordHash;
            RegistrationDate = DateTime.UtcNow;
            UpdatedDate = DateTime.UtcNow;
        }
        public Guid Id { get; private set; }
        public string FullName { get; private set; }

        private string _email = string.Empty;
        public string Email
        {
            get => _email;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Email cannot be empty");
                try
                {
                    var _ = new MailAddress(value); 
                }
                catch (FormatException)
                {
                    throw new ArgumentException("Email format is invalid");
                }
                _email = value.Trim().ToLower();
            }
        }
       
        public string PasswordHash { get; private set; } = null!;
        public string? Phone { get; private set; }
        public UserRole Role { get; private set; } = UserRole.User;
        public DateTimeOffset RegistrationDate { get; private set; }
        public DateTimeOffset UpdatedDate { get; private set; }

        public static User Create(string fullName, string email, string passwordHash) => new User(fullName, email, passwordHash);

        public void ChangeFullName(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentException("Full name cannot be empty");
            FullName = fullName.Trim();
            Touch();
        }

        public void ChangeEmail(string newEmail)
        {
            Email = newEmail;
            Touch();
        }

        public void ChangePassword(string newPasswordHash)
        {
            if (string.IsNullOrWhiteSpace(newPasswordHash))
                throw new ArgumentException("Password cannot be empty");
            PasswordHash = newPasswordHash;
            Touch();
        }

        public void ChangePhone(string? newPhone)
        {
            if (newPhone != null && string.IsNullOrWhiteSpace(newPhone))
                throw new ArgumentException("Phone cannot be empty");
            Phone = newPhone;
            Touch();
        }

        public void ChangeRole(UserRole newRole)
        {
            Role = newRole;
            Touch();
        }

        private void Touch() => UpdatedDate = DateTimeOffset.UtcNow;
    }

    // 🔐 Ролі користувачів
    public enum UserRole
    {
        User = 0,
        Employee = 1,
        Admin = 2,
    }

    // ================= AIRPORT =================
    public class Airport
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

    }
}
