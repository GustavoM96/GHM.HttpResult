<p align="center">
<img src="logo.png" alt="logo" width="200px"/>
</p>

<h1 align="center"> GHM.HTTPResult </h1>

GHM.HTTPResult aims to type the HTTP result of your API service or others.

## Install Package

.NET CLI

```sh
dotnet add package GHM.HttpResult
```

Package Manager

```sh
NuGet\Install-Package GHM.HttpResult
```

## Example

This lib has been created to improve description of method returns called, and facilitating the casting of success and error cases

```csharp
using GHM.HTTPResult;

public class UserService
{

    public Ok<string> GetUserName(GetUserNameRequest request)
    {
        User user =  _userRepo.GetUser(request.Id);

        if(user is null)
        {
            return Error.NotFound($"not found user by id {request.Id}");
        }

        return user.Name
    }

    public Ok<string> GetUserWithRailWay(GetUserNameRequest request)
    {
        return Ok.Create(request)
            .BindData((req) => _userRepo.GetUser(req.Id));
            .BindError((user) => (user is null, Error.NotFound($"not found user by id {request.Id}")))
            .Map((user) => user.Name);
    }
}

```

```csharp
using GHM.HTTPResult;

public class UserController : ControllerBase
{

    [HttpGet]
    public IActionResult GetUser(int id)
    {
        Ok<User> result = _userService.GetUser(id);

        return ConvertExempleTest(result);// you can create a Converter to change return from Result to Action automatically
    }

    [HttpGet]
    public IActionResult GetUser(int id)
    {
        Ok<User> result = _userService.GetUser(id);

        return result.Match(
            (data) => Ok(data),
            (errors) => BadRequest(errors)
        );// using a match pattern
    }
}

```

## Classes

### HttpSuccess

Type of HttpSuccess:

- Ok
- Created
- NoContent

```csharp
public class Ok<TData> : Result<TData> { }
public class Ok : Result<BasicResponse> { }

public class Created<TData> : Result<TData> { }
public class Created : Result<BasicResponse> { }

public class NoContent : Result { }

```

### Error

It's just one type of error with different HttpStatusCode:

- NotFound
- Forbidden
- BadRequest
- Unauthorized
- Conflict

```csharp
public class Error
{
    public string Title { get; init; }
    public HttpStatusCode StatusCode { get; init; }

    private Error(string title, HttpStatusCode httpStatusCode)
    {
        Title = title;
        StatusCode = httpStatusCode;
    }

    public static Error NotFound(string title) => new(title, HttpStatusCode.NotFound);

    public static Error Forbidden(string title) => new(title, HttpStatusCode.Forbidden);

    public static Error BadRequest(string title) => new(title, HttpStatusCode.BadRequest);

    public static Error Unauthorized(string title) => new(title, HttpStatusCode.Unauthorized);

    public static Error Conflict(string title) => new(title, HttpStatusCode.Conflict);
}

```

### ValidationError

It's just one type of validation with different ErrorHttpStatusCode or OkStatus:

- NotFound
- Forbidden
- BadRequest
- Unauthorized
- Conflict

```csharp
public class ValidationError
{
    public ValidationError(bool isError)
    {
        IsError = isError;
    }

    public bool IsError { get; init; }

    public Result AsNotFound(string errorTitle) => IsError ? new(Error.NotFound(errorTitle)) : Result.Successful;

    public Result AsForbidden(string errorTitle) => IsError ? new(Error.Forbidden(errorTitle)) : Result.Successful;

    public Result AsBadRequest(string errorTitle) => IsError ? new(Error.BadRequest(errorTitle)) : Result.Successful;

    public Result AsUnauthorized(string errorTitle) => IsError ? new(Error.Unauthorized(errorTitle)) : Result.Successful;

    public Result AsConflict(string errorTitle) => IsError ? new(Error.Conflict(errorTitle)) : Result.Successful;

    public Result AsError(Error error) => IsError ? new(error) : Result.Successful;
}
```

### Result

A abstract class base for Ok, Created and NoContent classes.
`Result` has properties base as Errors, StatusCode and IsSuccess.

```csharp
public abstract class Result
{
    public IReadOnlyList<Error> Errors { get; init; }

    public HttpStatusCode StatusCode { get; init; }

    public bool IsSuccess => (int)StatusCode < 400;
}
```

## Star

if you enjoy, don't forget the ⭐ and install the package 😊.
