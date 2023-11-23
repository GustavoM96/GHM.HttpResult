using System.Net;

namespace GHM.HttpResult;

public class HttpOk<TData> : HttpResult<TData>
{
    private HttpOk(TData data)
        : base(data, HttpStatusCode.OK) { }

    private HttpOk(HttpError error)
        : base(error) { }

    private HttpOk(List<HttpError> errors)
        : base(errors) { }

    public static implicit operator HttpOk<TData>(TData data) => new(data);

    public static implicit operator HttpOk<TData>(HttpError error) => new(error);

    public static implicit operator HttpOk<TData>(List<HttpError> errors) => new(errors);
}

public class HttpOk : HttpResult<BasicResult>
{
    private HttpOk(BasicResult basicResult)
        : base(basicResult, HttpStatusCode.OK) { }

    private HttpOk(HttpError error)
        : base(error) { }

    private HttpOk(List<HttpError> errors)
        : base(errors) { }

    public static HttpOk Success(string message = "") => new(new BasicResult(message));

    public static implicit operator HttpOk(string message) => new(new BasicResult(message));

    public static implicit operator HttpOk(HttpError error) => new(error);

    public static implicit operator HttpOk(List<HttpError> errors) => new(errors);
}

public class HttpCreated : HttpResult<BasicResult>
{
    private HttpCreated(BasicResult basicResult)
        : base(basicResult, HttpStatusCode.OK) { }

    private HttpCreated(HttpError error)
        : base(error) { }

    private HttpCreated(List<HttpError> errors)
        : base(errors) { }

    public static HttpCreated Success(string message = "") => new(new BasicResult(message));

    public static implicit operator HttpCreated(string message) => new(new BasicResult(message));

    public static implicit operator HttpCreated(HttpError error) => new(error);

    public static implicit operator HttpCreated(List<HttpError> errors) => new(errors);
}

public class HttpCreated<TData> : HttpResult<TData>
{
    private HttpCreated(TData data)
        : base(data, HttpStatusCode.Created) { }

    private HttpCreated(HttpError error)
        : base(error) { }

    private HttpCreated(List<HttpError> errors)
        : base(errors) { }

    public static implicit operator HttpCreated<TData>(TData data) => new(data);

    public static implicit operator HttpCreated<TData>(HttpError error) => new(error);

    public static implicit operator HttpCreated<TData>(List<HttpError> errors) => new(errors);
}

public class HttpNoContent : HttpResult
{
    public HttpNoContent()
        : base(HttpStatusCode.NoContent) { }

    private HttpNoContent(HttpError error)
        : base(error) { }

    private HttpNoContent(List<HttpError> errors)
        : base(errors) { }

    public static HttpNoContent Success() => new();

    public static implicit operator HttpNoContent(HttpError error) => new(error);

    public static implicit operator HttpNoContent(List<HttpError> errors) => new(errors);
}
