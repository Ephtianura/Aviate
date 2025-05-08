using System.Net;
using Aviate.Application.Exceptions;

namespace Aviate.API.Middleware.Exceptions;

public static class ExceptionGroups
{
    public static readonly Dictionary<HttpStatusCode, Type[]> Groups = new()
    {
        // Групи виключень по статусам
        [HttpStatusCode.NotFound] = new[]
        {
            typeof(KeyNotFoundException),
            typeof(FileNotFoundException)
        },
        //[HttpStatusCode.Conflict] = new[]
        //{
        //},
        [HttpStatusCode.Unauthorized] = new[]
        {
            typeof(UnauthorizedAccessException)
        },
        [HttpStatusCode.BadRequest] = new[]
        {
            typeof(InvalidOperationException),
            typeof(ArgumentException),
            typeof(EmailAlreadyExistsException),
            typeof(AuthenticationException)
        }
    };
}
