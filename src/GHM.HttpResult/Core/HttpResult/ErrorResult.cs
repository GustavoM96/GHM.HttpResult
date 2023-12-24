using System.Net;

namespace GHM.HttpResult;

public record ErrorResult(string Title, HttpStatusCode StatusCode, IReadOnlyList<Error> Errors)
{
    public DateTime ResponseAt { get; } = DateTime.Now;
}
