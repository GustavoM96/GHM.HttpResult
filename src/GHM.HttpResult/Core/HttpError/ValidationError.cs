using System.Net;

namespace GHM.HttpResult;

public class ValidationError
{
    public ValidationError(bool isError)
    {
        IsError = isError;
    }

    public bool IsError { get; set; }

    public Result AsNotFound(string errorTitle) => IsError ? new(Error.NotFound(errorTitle)) : Result.Successful;

    public Result AsForbidden(string errorTitle) => IsError ? new(Error.Forbidden(errorTitle)) : Result.Successful;

    public Result AsBadRequest(string errorTitle) => IsError ? new(Error.BadRequest(errorTitle)) : Result.Successful;

    public Result AsUnauthorized(string errorTitle) => IsError ? new(Error.Unauthorized(errorTitle)) : Result.Successful;

    public Result AsConflict(string errorTitle) => IsError ? new(Error.Conflict(errorTitle)) : Result.Successful;

    public Result AsError(Error error) => IsError ? new(error) : Result.Successful;
}
