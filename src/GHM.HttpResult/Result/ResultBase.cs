using System.Net;

namespace GHM.HttpResult.Result;

public abstract class HttpResult<TData> : HttpResult
{
    private readonly TData? _data;

    public TData Data => IsSuccess ? throw new ArgumentException("http error has no data") : _data!;

    public HttpResult(TData data, HttpStatusCode httpStatusCode)
        : base(httpStatusCode)
    {
        _data = data;
    }

    public HttpResult(Error error)
        : base(error) { }

    public HttpResult(List<Error> errors)
        : base(errors) { }
}

public abstract class HttpResult
{
    public IReadOnlyCollection<Error> Errors { get; init; }

    public HttpStatusCode HttpStatusCode { get; init; }

    public bool IsSuccess => (int)HttpStatusCode < 400;

    public HttpResult(HttpStatusCode httpStatusCode)
    {
        HttpStatusCode = httpStatusCode;
        Errors = Array.Empty<Error>();
    }

    public HttpResult(Error error)
    {
        HttpStatusCode = error.HttpStatusCode;
        Errors = new Error[1] { error };
    }

    public HttpResult(List<Error> errors)
    {
        HttpStatusCode = errors.Count == 1 ? errors.First().HttpStatusCode : HttpStatusCode.BadRequest;
        Errors = errors;
    }
}
