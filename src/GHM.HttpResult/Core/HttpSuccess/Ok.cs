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

    public static implicit operator Ok<TData>(TData data) => new(data);

    public static implicit operator Ok<TData>(Error error) => new(error);

    public static implicit operator Ok<TData>(List<Error> errors) => new(errors);
}

public static class Ok
{
    public static Ok<TData> Create<TData>(TData data) => new(data);
}
