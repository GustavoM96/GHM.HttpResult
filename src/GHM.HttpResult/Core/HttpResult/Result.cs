﻿using System.Net;

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

    protected void AddError(Error error)
    {
        StatusCode = error.StatusCode;
        _errors.Add(error);
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

    protected void TapResult<TData>(Action<TData> action, TData data)
    {
        if (IsSuccess)
        {
            action(data);
        }
    }

    protected (T? Data, bool Success) BindDataResult<TData, T>(Func<TData, T> action, TData data)
    {
        if (IsSuccess)
        {
            var newData = action(data);
            return (newData, true);
        }
        return (default, false);
    }

    protected void BindErrorResult<TData>(Func<TData, Result> action, TData data)
    {
        if (data is null)
        {
            return;
        }

        var result = action(data);
        if (!result.IsSuccess)
        {
            AddErrors(result.Errors);
        }
    }

    protected void BindErrorResult(bool isError, Error error)
    {
        if (isError)
        {
            AddError(error);
        }
    }

    protected static (T? Data, bool Success) MapResult<TData, T>(Func<TData, T> action, TData data)
    {
        if (data is null)
        {
            return (default, false);
        }

        var newData = action(data);
        return (newData, true);
    }
}
