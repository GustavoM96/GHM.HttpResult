using System.Net;

namespace GHM.HttpResult.Core;

public class NoContent<TData> : NoContent
{
    private readonly TData? _data;
    public TData Data => _data is not null ? _data! : throw new ArgumentException("http error has no data.");

    public NoContent(List<Error> errors)
        : base(errors) { }

    public NoContent(TData data)
        : base()
    {
        _data = data;
    }

    public NoContent(TData data, List<Error> errors)
        : base(errors)
    {
        _data = data;
    }

    public NoContent<TData> Tap(Action<TData> action)
    {
        if (IsSuccess)
        {
            action(Data);
        }
        return this;
    }

    public NoContent<TData> Bind(Func<TData, Result> action)
    {
        var result = action(Data);
        if (!result.IsSuccess)
        {
            AddErrors(result.Errors);
        }
        return this;
    }

    public NoContent<T> Map<T>(Func<TData, T> action)
    {
        var data = action(Data);
        return new(data, Errors.ToList());
    }
}

public class NoContent : Result
{
    public NoContent()
        : base(HttpStatusCode.NoContent) { }

    public NoContent(Error error)
        : base(error) { }

    public NoContent(List<Error> errors)
        : base(errors, HttpStatusCode.NoContent) { }

    public static NoContent<TData> Create<TData>(TData data) => new(data);

    public static NoContent Create() => new();

    public static implicit operator NoContent(Error error) => new(error);

    public static implicit operator NoContent(List<Error> errors) => new(errors);
}
