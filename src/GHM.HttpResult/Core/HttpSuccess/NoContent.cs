using System.Net;

namespace GHM.HttpResult.Core;

public class NoContent<TData> : NoContent
{
    private readonly TData? _data;
    public TData Data => _data!;

    public NoContent(IEnumerable<Error> errors)
        : base(errors) { }

    public NoContent(Error error)
        : base(error) { }

    public NoContent(TData data)
        : base()
    {
        _data = data;
    }

    public NoContent(TData data, IEnumerable<Error> errors)
        : base(errors)
    {
        _data = data;
    }

    public NoContent<TData> Tap(Action<TData> action)
    {
        TapResult(action, Data);
        return this;
    }

    public async Task<NoContent<TData>> Tap(Func<TData, Task> action)
    {
        await TapResultAsync(action, Data);
        return this;
    }

    public NoContent<T> BindData<T>(Func<TData, T> action)
    {
        var result = BindDataResult(action, Data);
        return result.Success ? new(result.Data!) : new(Errors);
    }

    public async Task<NoContent<T>> BindData<T>(Func<TData, Task<T>> action)
    {
        var result = await BindDataResultAsync(action, Data);
        return result.Success ? new(result.Data!) : new(Errors);
    }

    public NoContent<TData> BindError(Func<TData, Result> action)
    {
        BindErrorResult(action, Data);
        return this;
    }

    public async Task<NoContent<TData>> BindError(Func<TData, Task<Result>> action)
    {
        await BindErrorResultAsync(action, Data);
        return this;
    }

    public NoContent<TData> BindError(Func<TData, (bool, Error)> action)
    {
        BindErrorResult(action, Data);
        return this;
    }

    public NoContent<T> Map<T>(Func<TData, T> action)
    {
        var result = MapResult(action, Data);
        return result.Success ? new(result.Data!) : new(Errors);
    }

    public SuccessResult<TData> ToSuccessResult() => new(Data, StatusCode);

    public TResult Match<TResult>(Func<SuccessResult<TData>, TResult> onSuccess, Func<ErrorResult, TResult> onError)
    {
        return IsSuccess ? onSuccess(ToSuccessResult()) : onError(ToErrorResult());
    }

    public static implicit operator NoContent<TData>(TData data) => new(data);

    public static implicit operator NoContent<TData>(Error error) => new(error);

    public static implicit operator NoContent<TData>(List<Error> errors) => new(errors);

    public static implicit operator NoContent<TData>(Error[] errors) => new(errors);
}

public class NoContent : Result
{
    public NoContent()
        : base(HttpStatusCode.NoContent) { }

    public NoContent(Error error)
        : base(error) { }

    public NoContent(IEnumerable<Error> errors)
        : base(errors, HttpStatusCode.NoContent) { }

    public static NoContent<TData> Create<TData>(TData data) => new(data);

    public static NoContent Create() => new();

    public static implicit operator NoContent(Error error) => new(error);

    public static implicit operator NoContent(List<Error> errors) => new(errors);

    public static implicit operator NoContent(Error[] errors) => new(errors);
}
