using System.Net;

namespace GHM.HttpResult.Core;

public class Created<TData> : Result<TData>
{
    public Created(TData data)
        : base(data, HttpStatusCode.Created) { }

    public Created(Error error)
        : base(error) { }

    public Created(List<Error> errors)
        : base(errors, HttpStatusCode.Created) { }

    public Created(TData data, List<Error> errors)
        : base(data, errors, HttpStatusCode.Created) { }

    public Created<TData> Tap(Action<TData> action)
    {
        if (IsSuccess)
        {
            action(Data);
        }
        return this;
    }

    public Created<TData> Tap(Func<TData, Result<TData>> action)
    {
        if (IsSuccess)
        {
            var result = action(Data);
            return new(result.Data, result.Errors.ToList());
        }

        return this;
    }

    public Created<TData> Bind(Func<TData, Result<TData>> action)
    {
        var result = action(Data);
        return new(result.Data, result.Errors.ToList());
    }

    public Created<T> Map<T>(Func<TData, T> action)
    {
        var data = action(Data);
        return new(data, Errors.ToList());
    }

    public Created<TData> Check<T>(Func<TData, Result<T>> action)
    {
        var result = action(Data);

        if (!result.IsSuccess)
        {
            AddErrors(result.Errors);
        }

        return this;
    }

    public static implicit operator Created<TData>(TData data) => new(data);

    public static implicit operator Created<TData>(Error error) => new(error);

    public static implicit operator Created<TData>(List<Error> errors) => new(errors);
}

public static class Created
{
    public static Created<TData> Create<TData>(TData data) => new(data);
}
