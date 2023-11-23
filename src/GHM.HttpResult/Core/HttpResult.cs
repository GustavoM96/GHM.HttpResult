using System.Net;

namespace GHM.HttpResult;

public abstract class Result<TData> : Result
{
    private readonly TData? _data;

    public TData Data => IsSuccess ? _data! : throw new ArgumentException("http error has no data.");

    public Result(TData data, HttpStatusCode statusCode)
        : base(statusCode)
    {
        _data = data;
    }

    public Result(Error error)
        : base(error) { }

    public Result(List<Error> errors)
        : base(errors) { }

    public SuccessResult<TData> ToSuccessResult() => new(Data, StatusCode);

    public TResult Match<TResult>(Func<SuccessResult<TData>, TResult> onSuccess, Func<ErrorResult, TResult> onError)
    {
        return IsSuccess ? onSuccess(ToSuccessResult()) : onError(ToErrorResult());
    }
}

public abstract class Result
{
    public IReadOnlyList<Error> Errors { get; init; }

    public HttpStatusCode StatusCode { get; init; }

    public bool IsSuccess => (int)StatusCode < 400;

    public Result(HttpStatusCode statusCode)
    {
        StatusCode = statusCode;
        Errors = Array.Empty<Error>();
    }

    public Result(Error error)
    {
        StatusCode = error.StatusCode;
        Errors = new Error[1] { error };
    }

    public Result(List<Error> errors)
    {
        StatusCode = errors.Count == 1 ? errors.First().StatusCode : HttpStatusCode.BadRequest;
        Errors = errors;
    }

    public ErrorResult ToErrorResult()
    {
        if (!Errors.Any())
        {
            throw new ArgumentException("not found errors.");
        }

        var title = Errors.Count == 1 ? Errors[0].Title : "many error has occurred.";
        return new(title, StatusCode, Errors);
    }

    public TResult Match<TResult>(Func<TResult> onSuccess, Func<ErrorResult, TResult> onError)
    {
        return IsSuccess ? onSuccess() : onError(ToErrorResult());
    }
}
