using System.Net;

namespace GHM.HttpResult;

public class HttpError
{
    public string Title { get; init; }
    public HttpStatusCode StatusCode { get; init; }

    private HttpError(string title, HttpStatusCode httpStatusCode)
    {
        Title = title;
        StatusCode = httpStatusCode;
    }

    public static HttpError NotFound(string title) => new(title, HttpStatusCode.NotFound);

    public static HttpError Forbidden(string title) => new(title, HttpStatusCode.Forbidden);

    public static HttpError BadRequest(string title) => new(title, HttpStatusCode.BadRequest);

    public static HttpError Unauthorized(string title) => new(title, HttpStatusCode.Unauthorized);

    public static HttpError Conflict(string title) => new(title, HttpStatusCode.Conflict);
}
