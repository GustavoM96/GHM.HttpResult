namespace GHM.HttpResult;

public class ValidationResult
{
    public ValidationResult(bool isError)
    {
        IsError = isError;
    }

    public bool IsError { get; init; }

    public Result AsNotFound(string message) => IsError ? new(Error.NotFound(message)) : Result.Ok;

    public Result AsForbidden(string message) => IsError ? new(Error.Forbidden(message)) : Result.Ok;

    public Result AsBadRequest(string message) => IsError ? new(Error.BadRequest(message)) : Result.Ok;

    public Result AsUnauthorized(string message) => IsError ? new(Error.Unauthorized(message)) : Result.Ok;

    public Result AsConflict(string message) => IsError ? new(Error.Conflict(message)) : Result.Ok;

    public Result AsError(Error error) => IsError ? new(error) : Result.Ok;
}
