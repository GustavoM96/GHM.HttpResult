using System.Net;

namespace GHM.HttpResult;

public record ErrorResult(string Message, HttpStatusCode StatusCode, IReadOnlyList<Error> Errors)
{
    public DateTime ResponseAt { get; } = DateTime.Now;
}
