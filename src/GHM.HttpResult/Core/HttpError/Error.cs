using System.Net;

namespace GHM.HttpResult;

public record Error
{
    public string Message { get; init; }
    public HttpStatusCode StatusCode { get; init; }

    private Error(string message, HttpStatusCode httpStatusCode)
    {
        Message = message;
        StatusCode = httpStatusCode;
    }

    public static Error NotFound(string message) => new(message, HttpStatusCode.NotFound);

    public static Error Forbidden(string message) => new(message, HttpStatusCode.Forbidden);

    public static Error BadRequest(string message) => new(message, HttpStatusCode.BadRequest);

    public static Error Unauthorized(string message) => new(message, HttpStatusCode.Unauthorized);

    public static Error Conflict(string message) => new(message, HttpStatusCode.Conflict);
}
