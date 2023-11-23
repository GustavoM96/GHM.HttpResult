using System.Net;

namespace GHM.HttpResult;

public class Ok<TData> : Result<TData>
{
    private Ok(TData data)
        : base(data, HttpStatusCode.OK) { }

    private Ok(Error error)
        : base(error) { }

    private Ok(List<Error> errors)
        : base(errors) { }

    public static implicit operator Ok<TData>(TData data) => new(data);

    public static implicit operator Ok<TData>(Error error) => new(error);

    public static implicit operator Ok<TData>(List<Error> errors) => new(errors);
}

public class Ok : Result<BasicResponse>
{
    private Ok(BasicResponse basicResult)
        : base(basicResult, HttpStatusCode.OK) { }

    private Ok(Error error)
        : base(error) { }

    private Ok(List<Error> errors)
        : base(errors) { }

    public static Ok Success(string message = "") => new(new BasicResponse(message));

    public static implicit operator Ok(string message) => new(new BasicResponse(message));

    public static implicit operator Ok(Error error) => new(error);

    public static implicit operator Ok(List<Error> errors) => new(errors);
}

public class Created : Result<BasicResponse>
{
    private Created(BasicResponse basicResult)
        : base(basicResult, HttpStatusCode.OK) { }

    private Created(Error error)
        : base(error) { }

    private Created(List<Error> errors)
        : base(errors) { }

    public static Created Success(string message = "") => new(new BasicResponse(message));

    public static implicit operator Created(string message) => new(new BasicResponse(message));

    public static implicit operator Created(Error error) => new(error);

    public static implicit operator Created(List<Error> errors) => new(errors);
}

public class Created<TData> : Result<TData>
{
    private Created(TData data)
        : base(data, HttpStatusCode.Created) { }

    private Created(Error error)
        : base(error) { }

    private Created(List<Error> errors)
        : base(errors) { }

    public static implicit operator Created<TData>(TData data) => new(data);

    public static implicit operator Created<TData>(Error error) => new(error);

    public static implicit operator Created<TData>(List<Error> errors) => new(errors);
}

public class NoContent : Result
{
    public NoContent()
        : base(HttpStatusCode.NoContent) { }

    private NoContent(Error error)
        : base(error) { }

    private NoContent(List<Error> errors)
        : base(errors) { }

    public static NoContent Success() => new();

    public static implicit operator NoContent(Error error) => new(error);

    public static implicit operator NoContent(List<Error> errors) => new(errors);
}
