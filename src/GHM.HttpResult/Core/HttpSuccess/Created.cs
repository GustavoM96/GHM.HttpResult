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
        TapResult(action, Data);
        return this;
    }

    public async Task<Created<TData>> TapAsync(Func<TData, Task> action)
    {
        await TapResultAsync(action, Data);
        return this;
    }

    public Created<T> BindData<T>(Func<TData, T> action)
    {
        var result = BindDataResult(action, Data);
        return result.Success ? new(result.Data!) : new(Errors);
    }

    public async Task<Created<T>> BindDataAsync<T>(Func<TData, Task<T>> action)
    {
        var result = await BindDataResultAsync(action, Data);
        return result.Success ? new(result.Data!) : new(Errors);
    }

    public Created<TData> BindError(Func<TData, Result> action)
    {
        BindErrorResult(action, Data);
        return this;
    }

    public async Task<Created<TData>> BindErrorAsync(Func<TData, Task<Result>> action)
    {
        await BindErrorResultAsync(action, Data);
        return this;
    }

    public Created<TData> BindError(Func<TData, (bool, Error)> action)
    {
        BindErrorResult(action, Data);
        return this;
    }

    public Created<T> Map<T>(Func<TData, T> action)
    {
        var result = MapResult(action, Data);
        return result.Success ? new(result.Data!) : new(Errors);
    }

    public SuccessResult<TData> ToSuccessResult() => new(Data, StatusCode);

    public TResult Match<TResult>(Func<SuccessResult<TData>, TResult> onSuccess, Func<ErrorResult, TResult> onError)
    {
        return IsSuccess ? onSuccess(ToSuccessResult()) : onError(ToErrorResult());
    }

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
