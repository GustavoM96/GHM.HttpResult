using System.Net;

namespace GHM.HttpResult;

public class NoContent<TData> : Result<TData>
{
    public NoContent(TData data)
        : base(data, HttpStatusCode.NoContent) { }

    public NoContent(Error error)
        : base(error) { }

    public NoContent(IEnumerable<Error> errors)
        : base(errors, HttpStatusCode.NoContent) { }

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

    public static NoContent Create => new();

    public static implicit operator NoContent(Error error) => new(error);

    public static implicit operator NoContent(List<Error> errors) => new(errors);

    public static implicit operator NoContent(Error[] errors) => new(errors);
}
