using System.Net;

namespace GHM.HttpResult;

public record SuccessResult<TData>(TData Data, HttpStatusCode StatusCode)
{
    public DateTime ResponseAt { get; } = DateTime.Now;
}

public record SuccessResult(HttpStatusCode StatusCode)
{
    public DateTime ResponseAt { get; } = DateTime.Now;
}
