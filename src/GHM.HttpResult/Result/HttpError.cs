using System.Net;

namespace GHM.HttpResult.Result;

public class NotFound : HttpError
{
    public NotFound(string title)
        : base(title, HttpStatusCode.NotFound) { }
}

public class BadRequest : HttpError
{
    public BadRequest(string title)
        : base(title, HttpStatusCode.BadRequest) { }
}

public class Conflict : HttpError
{
    public Conflict(string title)
        : base(title, HttpStatusCode.Conflict) { }
}

public class Forbidden : HttpError
{
    public Forbidden(string title)
        : base(title, HttpStatusCode.Forbidden) { }
}

public class Unauthorized : HttpError
{
    public Unauthorized(string title)
        : base(title, HttpStatusCode.Unauthorized) { }
}
