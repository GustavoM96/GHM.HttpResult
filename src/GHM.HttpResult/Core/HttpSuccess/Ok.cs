using System.Net;

namespace GHM.HttpResult.Core;

public class Ok<TData> : Result<TData>
{
    public Ok(TData data)
        : base(data, HttpStatusCode.OK) { }

    public Ok(Error error)
        : base(error) { }

    public Ok(List<Error> errors)
        : base(errors, HttpStatusCode.OK) { }

    public Ok(TData data, List<Error> errors)
        : base(data, errors, HttpStatusCode.OK) { }

    public Ok<TData> Tap(Action<TData> action)
    {
        if (IsSuccess)
        {
            action(Data);
        }
        return this;
    }

    public Ok<TData> ErrorIf(Func<TData, Result> action)
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
        var result = action(Data);
        return new Ok<T>(result);
    }

    public Ok<TData> Bind(Func<TData, Result<TData>> action)
    {
        var result = action(Data);
        return new(result.Data, result.Errors.ToList());
    }

    public static implicit operator Ok<TData>(TData data) => new(data);

    public static implicit operator Ok<TData>(Error error) => new(error);

    public static implicit operator Ok<TData>(List<Error> errors) => new(errors);
}

public static class Ok
{
    public static Ok<TData> Create<TData>(TData data) => new(data);
}
