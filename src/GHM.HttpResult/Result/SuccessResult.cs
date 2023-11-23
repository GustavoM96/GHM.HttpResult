using System.Net;

namespace GHM.HttpResult.Result;

public class HttpOk<TData> : HttpResult<TData>
{
    private HttpOk(TData data)
        : base(data, HttpStatusCode.OK) { }

    private HttpOk(Error error)
        : base(error) { }

    private HttpOk(List<Error> errors)
        : base(errors) { }

    public static implicit operator HttpOk<TData>(TData data) => new(data);

    public static implicit operator HttpOk<TData>(Error error) => new(error);

    public static implicit operator HttpOk<TData>(List<Error> errors) => new(errors);
}

public class HttpCreated<TData> : HttpResult<TData>
{
    private HttpCreated(TData data)
        : base(data, HttpStatusCode.Created) { }

    private HttpCreated(Error error)
        : base(error) { }

    private HttpCreated(List<Error> errors)
        : base(errors) { }

    public static implicit operator HttpCreated<TData>(TData data) => new(data);

    public static implicit operator HttpCreated<TData>(Error error) => new(error);

    public static implicit operator HttpCreated<TData>(List<Error> errors) => new(errors);
}

public class HttpNoContent : HttpResult
{
    public HttpNoContent()
        : base(HttpStatusCode.NoContent) { }

    private HttpNoContent(Error error)
        : base(error) { }

    private HttpNoContent(List<Error> errors)
        : base(errors) { }

    public static HttpNoContent Success() => new();

    public static implicit operator HttpNoContent(Error error) => new(error);

    public static implicit operator HttpNoContent(List<Error> errors) => new(errors);
}
