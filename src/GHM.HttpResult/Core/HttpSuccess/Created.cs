using System.Net;

namespace GHM.HttpResult.Core;

public class Created<TData> : Created
{
    private readonly TData? _data;
    public TData Data => _data!;

    public Created(TData data)
        : base()
    {
        _data = data;
    }

    public Created(Error error)
        : base(error) { }

    public Created(IEnumerable<Error> errors)
        : base(errors) { }

    public Created(TData data, IEnumerable<Error> errors)
        : base(errors)
    {
        _data = data;
    }

    public Created<TData> Tap(Action<TData> action)
    {
        if (IsSuccess)
        {
            action(Data);
        }
        return this;
    }

    public Created<T> BindData<T>(Func<TData, T> action)
    {
        if (IsSuccess)
        {
            var data = action(Data);
            return new(data);
        }
        return new(Errors.ToList());
    }

    public Created<TData> BindError(Func<TData, Result> action)
    {
        var result = action(Data);
        if (!result.IsSuccess)
        {
            AddErrors(result.Errors);
        }
        return this;
    }

    public Created<T> Map<T>(Func<TData, T> action)
    {
        var data = action(Data);
        return new(data, Errors);
    }

    public SuccessResult<TData> ToSuccessResult() => new(Data, StatusCode);

    public TResult Match<TResult>(Func<SuccessResult<TData>, TResult> onSuccess, Func<ErrorResult, TResult> onError)
    {
        return IsSuccess ? onSuccess(ToSuccessResult()) : onError(ToErrorResult());
    }

    public Created ToCreated() => new(Errors);

    public static implicit operator Created<TData>(TData data) => new(data);

    public static implicit operator Created<TData>(Error error) => new(error);

    public static implicit operator Created<TData>(List<Error> errors) => new(errors);

    public static implicit operator Created<TData>(Error[] errors) => new(errors);
}

public class Created : Result
{
    public Created()
        : base(HttpStatusCode.Created) { }

    public Created(Error error)
        : base(error) { }

    public Created(IEnumerable<Error> errors)
        : base(errors, HttpStatusCode.Created) { }

    public static Created<TData> Create<TData>(TData data) => new(data);

    public static Created Create() => new();

    public static implicit operator Created(Error error) => new(error);

    public static implicit operator Created(List<Error> errors) => new(errors);

    public static implicit operator Created(Error[] errors) => new(errors);
}
