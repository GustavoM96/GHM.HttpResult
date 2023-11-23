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

    public SuccessResult<TData> ToSuccessResult() => new(Data, StatusCode);

    public ErrorResult ToErrorResult()
    {
        if (!Errors.Any())
        {
            throw new ArgumentException("not found errors");
        }

        var title = Errors.Count == 1 ? Errors[0].Title : "many error has occured";

        return new(title, StatusCode, Errors);
    }
}

public abstract class HttpResult
{
    public IReadOnlyList<Error> Errors { get; init; }

    public HttpStatusCode StatusCode { get; init; }

    public bool IsSuccess => (int)StatusCode < 400;

    public HttpResult(HttpStatusCode httpStatusCode)
    {
        StatusCode = httpStatusCode;
        Errors = Array.Empty<Error>();
    }

    public HttpResult(Error error)
    {
        StatusCode = error.HttpStatusCode;
        Errors = new Error[1] { error };
    }

    public HttpResult(List<Error> errors)
    {
        StatusCode = errors.Count == 1 ? errors.First().HttpStatusCode : HttpStatusCode.BadRequest;
        Errors = errors;
    }
}
