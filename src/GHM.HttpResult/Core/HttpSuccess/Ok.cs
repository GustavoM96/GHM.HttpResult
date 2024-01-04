using System.Net;

namespace GHM.HttpResult.Core;

public class Ok<TData> : Ok
{
    private readonly TData? _data;
    public TData Data => _data!;

    public Ok(TData data)
        : base()
    {
        _data = data;
    }

    public Ok(Error error)
        : base(error) { }

    public Ok(IEnumerable<Error> errors)
        : base(errors) { }

    public Ok(TData data, IEnumerable<Error> errors)
        : base(errors)
    {
        _data = data;
    }

    public Ok<TData> Tap(Action<TData> action)
    {
        if (IsSuccess)
        {
            action(Data);
        }
        return this;
    }

    public Ok<T> BindData<T>(Func<TData, T> action)
    {
        if (IsSuccess)
        {
            var data = action(Data);
            return new(data);
        }
        return new(Errors.ToList());
    }

    public Ok<TData> BindError(Func<TData, Result> action)
    {
        var result = action(Data);
        if (!result.IsSuccess)
        {
            AddErrors(result.Errors);
        }
        return this;
    }

    public Ok<T> Map<T>(Func<TData, T> action)
    {
        var data = action(Data);
        return new(data, Errors.ToList());
    }

    public SuccessResult<TData> ToSuccessResult() => new(Data, StatusCode);

    public TResult Match<TResult>(Func<SuccessResult<TData>, TResult> onSuccess, Func<ErrorResult, TResult> onError)
    {
        return IsSuccess ? onSuccess(ToSuccessResult()) : onError(ToErrorResult());
    }

    public Ok ToOk() => new(Errors.ToList());

    public static implicit operator Ok<TData>(TData data) => new(data);

    public static implicit operator Ok<TData>(Error error) => new(error);

    public static implicit operator Ok<TData>(List<Error> errors) => new(errors);

    public static implicit operator Ok<TData>(Error[] errors) => new(errors);
}

public class Ok : Result
{
    public Ok()
        : base(HttpStatusCode.OK) { }

    public Ok(Error error)
        : base(error) { }

    public Ok(IEnumerable<Error> errors)
        : base(errors, HttpStatusCode.OK) { }

    public static Ok<TData> Create<TData>(TData data) => new(data);

    public static Ok Create() => new();

    public static implicit operator Ok(Error error) => new(error);

    public static implicit operator Ok(List<Error> errors) => new(errors);

    public static implicit operator Ok(Error[] errors) => new(errors);
}
