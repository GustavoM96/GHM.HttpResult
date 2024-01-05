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
        TapResult(action, Data);
        return this;
    }

    public async Task<Ok<TData>> Tap(Func<TData, Task> action)
    {
        await TapResult(action, Data);
        return this;
    }

    public Ok<T> BindData<T>(Func<TData, T> action)
    {
        var result = BindDataResult(action, Data);
        return result.Success ? new(result.Data!) : new(Errors);
    }

    public async Task<Ok<T>> BindData<T>(Func<TData, Task<T>> action)
    {
        var result = await BindDataResult(action, Data);
        return result.Success ? new(result.Data!) : new(Errors);
    }

    public Ok<TData> BindError(Func<TData, Result> action)
    {
        BindErrorResult(action, Data);
        return this;
    }

    public async Task<Ok<TData>> BindError(Func<TData, Task<Result>> action)
    {
        await BindErrorResult(action, Data);
        return this;
    }

    public Ok<TData> BindError(Func<TData, (bool, Error)> action)
    {
        BindErrorResult(action, Data);
        return this;
    }

    public Ok<T> Map<T>(Func<TData, T> action)
    {
        var result = MapResult(action, Data);
        return result.Success ? new(result.Data!) : new(Errors);
    }

    public SuccessResult<TData> ToSuccessResult() => new(Data, StatusCode);

    public TResult Match<TResult>(Func<SuccessResult<TData>, TResult> onSuccess, Func<ErrorResult, TResult> onError)
    {
        return IsSuccess ? onSuccess(ToSuccessResult()) : onError(ToErrorResult());
    }

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
