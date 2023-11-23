using System.Net;

namespace GHM.HttpResult;

public class Error
{
    public string Title { get; init; }
    public HttpStatusCode StatusCode { get; init; }

    private Error(string title, HttpStatusCode httpStatusCode)
    {
        Title = title;
        StatusCode = httpStatusCode;
    }

    public static Error NotFound(string title) => new(title, HttpStatusCode.NotFound);

    public static Error Forbidden(string title) => new(title, HttpStatusCode.Forbidden);

    public static Error BadRequest(string title) => new(title, HttpStatusCode.BadRequest);

    public static Error Unauthorized(string title) => new(title, HttpStatusCode.Unauthorized);

    public static Error Conflict(string title) => new(title, HttpStatusCode.Conflict);
}
