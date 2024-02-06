using System.Net;

namespace GHM.HttpResult;

public class Ok<TData> : Result<TData>
{
    public Ok(TData data)
        : base(data, HttpStatusCode.OK) { }

    public Ok(Error error)
        : base(error) { }

    public Ok(IEnumerable<Error> errors)
        : base(errors, HttpStatusCode.OK) { }

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

    public static Ok Create => new();

    public static implicit operator Ok(Error error) => new(error);

    public static implicit operator Ok(List<Error> errors) => new(errors);

    public static implicit operator Ok(Error[] errors) => new(errors);
}
