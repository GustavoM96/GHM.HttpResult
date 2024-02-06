﻿using System.Net;

namespace GHM.HttpResult;

public class Result
{
    protected readonly List<Error> _errors = new();
    public IReadOnlyList<Error> Errors => _errors;
    public bool IsValid => !_errors.Any();

    public string Message =>
        _errors.Count switch
        {
            0 => "OK",
            1 => Errors[0].Message,
            _ => "many error has occurred."
        };

    public Result(IEnumerable<Error> errors)
    {
        _errors = errors.ToList();
        StatusCode = SetErrorStatus(_errors);
    }

    public Result(IEnumerable<Error> errors, HttpStatusCode statusCode)
    {
        _errors = errors.ToList();
        StatusCode = _errors.Any() ? SetErrorStatus(_errors) : statusCode;
    }

    public Result(Error error)
    {
        _errors = new List<Error> { error };
        StatusCode = error.StatusCode;
    }

    public HttpStatusCode StatusCode { get; private set; }

    public Result(HttpStatusCode statusCode)
    {
        StatusCode = statusCode;
    }

    private static HttpStatusCode SetErrorStatus(List<Error> errors)
    {
        return errors.Count == 1 ? errors.First().StatusCode : HttpStatusCode.BadRequest;
    }

    public void AddErrors(IEnumerable<Error> errors)
    {
        _errors.AddRange(errors.ToList());
        StatusCode = SetErrorStatus(_errors);
    }

    public void AddError(Error error)
    {
        _errors.Add(error);
        StatusCode = SetErrorStatus(_errors);
    }

    public static Result Ok => new(HttpStatusCode.OK);

    public static ValidationResult Validate(bool isError) => new(isError);

    public ErrorResult ToErrorResult()
    {
        return Errors.Any() ? new(Message, StatusCode, Errors) : throw new ArgumentException("not found errors.");
    }

    public SuccessResult ToSuccessResult() => new(StatusCode);

    public TResult Match<TResult>(Func<SuccessResult, TResult> onSuccess, Func<ErrorResult, TResult> onError)
    {
        return IsValid ? onSuccess(ToSuccessResult()) : onError(ToErrorResult());
    }
}

public class Result<TData> : Result
{
    private readonly TData? _data;
    public TData Data => _data!;

    public Result(TData data, HttpStatusCode statusCode)
        : base(statusCode)
    {
        _data = data;
    }

    public Result(Error error)
        : base(error) { }

    public Result(IEnumerable<Error> errors)
        : base(errors) { }

    public Result(IEnumerable<Error> errors, HttpStatusCode statusCode)
        : base(errors, statusCode) { }

    public SuccessResult<TData> ToSuccessDataResult() => new(Data, StatusCode);

    public TResult Match<TResult>(Func<SuccessResult<TData>, TResult> onSuccess, Func<ErrorResult, TResult> onError)
    {
        return IsValid ? onSuccess(ToSuccessDataResult()) : onError(ToErrorResult());
    }
}
