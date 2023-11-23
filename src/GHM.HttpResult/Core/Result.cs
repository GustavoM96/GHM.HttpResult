using System.Net;

namespace GHM.HttpResult;

public record SuccessResult<TData>(TData Data, HttpStatusCode StatusCode)
{
    public DateTime ResponseAt { get; } = DateTime.Now;
}

public record ErrorResult(string Title, HttpStatusCode StatusCode, IReadOnlyList<Error> Errors)
{
    public DateTime ResponseAt { get; } = DateTime.Now;
}

public record BasicResponse(string Message) { }
