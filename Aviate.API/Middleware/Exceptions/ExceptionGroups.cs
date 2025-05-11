using System.Net;
using Aviate.Application.Exceptions;

namespace Aviate.API.Middleware.Exceptions;

public static class ExceptionGroups
{
    public static readonly Dictionary<HttpStatusCode, Type[]> Groups = new()
    {
        // Групи виключень по статусам
        [HttpStatusCode.NotFound] =
        [
            typeof(KeyNotFoundException),
            typeof(FileNotFoundException)
        ],
        //[HttpStatusCode.Conflict] = new[]
        //{
        //},
        [HttpStatusCode.Unauthorized] =
        [
            typeof(UnauthorizedAccessException),
            typeof(MissingUserIdClaimException)
        ],
        [HttpStatusCode.BadRequest] =
        [
            typeof(InvalidOperationException),
            typeof(ArgumentException),
            typeof(EmailAlreadyExistsException),
            typeof(AuthenticationException),
            typeof(InvalidUserIdFormatException)
            
        ]
    };
}
