using System.Net;

namespace GHM.HttpResult;

public class Result
{
    private readonly List<Error> _errors;
    public IReadOnlyList<Error> Errors => _errors;

    public HttpStatusCode StatusCode { get; private set; }

    public bool IsSuccess => (int)StatusCode < 400;

    public Result(HttpStatusCode statusCode)
    {
        StatusCode = statusCode;
        _errors = new List<Error> { };
    }

    public Result(IEnumerable<Error> errors, HttpStatusCode statusCode)
    {
        StatusCode = errors.Any() ? errors.First().StatusCode : statusCode;
        _errors = errors.ToList();
    }

    public Result(Error error)
    {
        StatusCode = error.StatusCode;
        _errors = new List<Error> { error };
    }

    protected void AddErrors(IEnumerable<Error> errors)
    {
        StatusCode = errors.Any() ? errors.First().StatusCode : HttpStatusCode.BadRequest;
        _errors.AddRange(errors.ToList());
    }

    public static Result Successful => new(HttpStatusCode.OK);

    public static ValidationError Validate(bool isError) => new(isError);

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
