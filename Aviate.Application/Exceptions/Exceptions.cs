    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace Aviate.Application.Exceptions
    {
        // Вже існує ел. адреса
        public class EmailAlreadyExistsException : Exception
        {
            public EmailAlreadyExistsException(string email)
                : base($"User with email '{email}' already exists.") { }
        }

        // Невдала авторизація
        public class AuthenticationException : Exception
        {
            public AuthenticationException()
                : base("Invalid email or password.") { }
        }
    }
