using System.Net;

namespace GHM.HttpResult.Result;

public record SuccessResult<TData>(TData Data, HttpStatusCode StatusCode)
{
    public DateTime ResponseAt = DateTime.Now;
}

public record ErrorResult(string Title, HttpStatusCode StatusCode, IReadOnlyList<Error> Errors)
{
    public DateTime ResponseAt = DateTime.Now;
}

public record BasicResult(string Message) { }
