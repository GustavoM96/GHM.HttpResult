using System.Collections.ObjectModel;
using System.Net;

namespace GHM.HttpResult.Result;

public abstract class HttpResult<TData>
{
    private readonly TData? _data;
    private IReadOnlyCollection<HttpError> Errors { get; init; }

    public TData Data => IsSuccess ? throw new ArgumentException("http error has no data") : _data!;

    public HttpStatusCode HttpStatusCode { get; init; }

    public bool IsSuccess => (int)HttpStatusCode < 400;

    public HttpResult(TData data, HttpStatusCode httpStatusCode)
    {
        HttpStatusCode = httpStatusCode;
        _data = data;
        Errors = Array.Empty<HttpError>();
    }

    public HttpResult(HttpError error)
    {
        HttpStatusCode = error.HttpStatusCode;
        Errors = new HttpError[1] { error };
    }

    public HttpResult(List<HttpError> errors)
    {
        HttpStatusCode = errors.Count == 1 ? errors.First().HttpStatusCode : HttpStatusCode.BadRequest;
        Errors = errors;
    }
}

public abstract class HttpResult
{
    public HttpStatusCode HttpStatusCode { get; init; }

    public HttpResult(HttpStatusCode httpStatusCode)
    {
        HttpStatusCode = httpStatusCode;
    }
}

public abstract class HttpError
{
    public string Title { get; init; }
    public HttpStatusCode HttpStatusCode { get; init; }

    public HttpError(string title, HttpStatusCode httpStatusCode)
    {
        Title = title;
        HttpStatusCode = httpStatusCode;
    }
}
