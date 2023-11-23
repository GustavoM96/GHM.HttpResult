using System.Net;

namespace GHM.HttpResult.Result;

public class OkResult<TData> : HttpResult<TData>
{
    public OkResult(TData data)
        : base(data, HttpStatusCode.OK) { }

    public OkResult(HttpError error)
        : base(error) { }

    public OkResult(List<HttpError> errors)
        : base(errors) { }

    public static implicit operator OkResult<TData>(TData data) => new(data);

    public static implicit operator OkResult<TData>(HttpError errorBase) => new(errorBase);
}

public class CreatedResult<TData> : HttpResult<TData>
{
    public CreatedResult(TData data)
        : base(data, HttpStatusCode.Created) { }

    public static implicit operator CreatedResult<TData>(TData data)
    {
        return new CreatedResult<TData>(data);
    }
}

public class NoContentResult : HttpResult
{
    public NoContentResult()
        : base(HttpStatusCode.NoContent) { }
}
