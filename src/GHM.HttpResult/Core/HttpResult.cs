using System.Net;

namespace GHM.HttpResult;

public abstract class HttpResult<TData> : HttpResult
{
    private readonly TData? _data;

    public TData Data => IsSuccess ? _data! : throw new ArgumentException("http error has no data.");

    public HttpResult(TData data, HttpStatusCode statusCode)
        : base(statusCode)
    {
        _data = data;
    }

    public HttpResult(HttpError error)
        : base(error) { }

    public HttpResult(List<HttpError> errors)
        : base(errors) { }

    public SuccessResult<TData> ToSuccessResult() => new(Data, StatusCode);

    public ErrorResult ToErrorResult()
    {
        if (!Errors.Any())
        {
            throw new ArgumentException("not found errors.");
        }

        var title = Errors.Count == 1 ? Errors[0].Title : "many error has occurred.";
        return new(title, StatusCode, Errors);
    }

    public TResult Match<TResult>(Func<TData, TResult> onSuccess, Func<IReadOnlyList<HttpError>, TResult> onError)
    {
        return IsSuccess ? onSuccess(Data) : onError(Errors);
    }
}

public abstract class HttpResult
{
    public IReadOnlyList<HttpError> Errors { get; init; }

    public HttpStatusCode StatusCode { get; init; }

    public bool IsSuccess => (int)StatusCode < 400;

    public HttpResult(HttpStatusCode statusCode)
    {
        StatusCode = statusCode;
        Errors = Array.Empty<HttpError>();
    }

    public HttpResult(HttpError error)
    {
        StatusCode = error.StatusCode;
        Errors = new HttpError[1] { error };
    }

    public HttpResult(List<HttpError> errors)
    {
        StatusCode = errors.Count == 1 ? errors.First().StatusCode : HttpStatusCode.BadRequest;
        Errors = errors;
    }

    public TResult Match<TResult>(Func<TResult> onSuccess, Func<IReadOnlyList<HttpError>, TResult> onError)
    {
        return IsSuccess ? onSuccess() : onError(Errors);
    }
}
