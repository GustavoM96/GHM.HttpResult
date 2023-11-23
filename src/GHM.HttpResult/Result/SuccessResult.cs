using System.Net;

namespace GHM.HttpResult.Result;

public class OkResult<TData> : HttpResult<TData>
{
    private OkResult(TData data)
        : base(data, HttpStatusCode.OK) { }

    private OkResult(Error error)
        : base(error) { }

    private OkResult(List<Error> errors)
        : base(errors) { }

    public static implicit operator OkResult<TData>(TData data) => new(data);

    // public static implicit operator IActionResult(OkResult<TData> okResult) => "";

    public static implicit operator OkResult<TData>(Error error) => new(error);

    public static implicit operator OkResult<TData>(List<Error> errors) => new(errors);
}

public class CreatedResult<TData> : HttpResult<TData>
{
    private CreatedResult(TData data)
        : base(data, HttpStatusCode.Created) { }

    private CreatedResult(Error error)
        : base(error) { }

    private CreatedResult(List<Error> errors)
        : base(errors) { }

    public static implicit operator CreatedResult<TData>(TData data) => new(data);

    public static implicit operator CreatedResult<TData>(Error error) => new(error);

    public static implicit operator CreatedResult<TData>(List<Error> errors) => new(errors);
}

public class NoContentResult : HttpResult
{
    public NoContentResult()
        : base(HttpStatusCode.NoContent) { }

    private NoContentResult(Error error)
        : base(error) { }

    private NoContentResult(List<Error> errors)
        : base(errors) { }

    public static NoContentResult Success() => new();

    public static implicit operator NoContentResult(Error error) => new(error);

    public static implicit operator NoContentResult(List<Error> errors) => new(errors);
}
