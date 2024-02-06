using System.Net;

namespace GHM.HttpResult;

public class Created<TData> : Result<TData>
{
    public Created(TData data)
        : base(data, HttpStatusCode.Created) { }

    public Created(Error error)
        : base(error) { }

    public Created(IEnumerable<Error> errors)
        : base(errors, HttpStatusCode.Created) { }

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

    public static Created Create => new();

    public static implicit operator Created(Error error) => new(error);

    public static implicit operator Created(List<Error> errors) => new(errors);

    public static implicit operator Created(Error[] errors) => new(errors);
}
