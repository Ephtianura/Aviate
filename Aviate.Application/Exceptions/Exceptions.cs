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

    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string entityName, Guid id)
            : base($"{entityName} with id {id} not found.") { }
        public EntityNotFoundException(string entityName)
            : base($"{entityName} not found.") { }
    }
    public class EntityAlreadyExistsException : Exception
    {
        public EntityAlreadyExistsException()
            : base() { }
        public EntityAlreadyExistsException(string message)
        : base(message) { }
        public EntityAlreadyExistsException(string entityName, object key)
        : base($"{entityName} with key '{key}' already exists.") { }
    }
}
