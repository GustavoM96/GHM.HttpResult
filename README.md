<p align="center">
<img src="logo.png" alt="logo" width="200px"/>
</p>

<h1 align="center"> GHM.HTTPResult </h1>

[![Build & Test](https://github.com/GustavoM96/GHM.HttpResult/actions/workflows/build.yml/badge.svg)](https://github.com/GustavoM96/GHM.HttpResult/actions/workflows/build.yml)

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
public class Ok : Result { }

public class Created<TData> : Result<TData>  { }
public class Created : Result { }

public class NoContent<TData> : Result<TData>  { }
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
    public string Message { get; init; }
    public HttpStatusCode StatusCode { get; init; }

    private Error(string message, HttpStatusCode httpStatusCode)
    {
        Message = message;
        StatusCode = httpStatusCode;
    }

    public static Error NotFound(string message) => new(message, HttpStatusCode.NotFound);

    public static Error Forbidden(string message) => new(message, HttpStatusCode.Forbidden);

    public static Error BadRequest(string message) => new(message, HttpStatusCode.BadRequest);

    public static Error Unauthorized(string message) => new(message, HttpStatusCode.Unauthorized);

    public static Error Conflict(string message) => new(message, HttpStatusCode.Conflict);
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
```

### Result

A class base for Ok, Created and NoContent classes.
`Result` has properties base as Errors, StatusCode and IsSuccess.

```csharp
public class Result
{
    protected readonly List<Error> _errors = new();
    public IReadOnlyList<Error> Errors => _errors;
    public bool IsValid => !_errors.Any();
}

public class Result<TData> : Result
{
     public TData Data { get; init; } = default!;
}
```

## Star

if you enjoy, don't forget the ⭐ and install the package 😊.
