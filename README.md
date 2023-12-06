<p align="center">
<img src="logo.png" alt="logo" width="200px"/>
</p>

<h1 align="center"> GHM.HTTPResult </h1>

GHM.HTTPResult aims to type the HTTP result of your API service or others.

## Install Package

.NET CLI

```sh
dotnet add package GHM.HttpResult --version 1.0.0
```

Package Manager

```sh
NuGet\Install-Package GHM.HttpResult -Version 1.0.0
```

## Example

This lib has been created to improve description of method returns called, and facilitating the casting of success and error cases

```csharp
using GHM.HTTPResult
public class UserService
{

    public Ok<User> GetUser(int id)
    {
        User user = _userRepo.GetUser(id);

        if(user is null)
        {
            return Error.NotFound($"not found user by id {id}");
        }
        return user
    }
}

```

```csharp
using GHM.HTTPResult
public class UserController : ControllerBase
{

    [HttpGet]
    public IActionResult GetUser(int id)
    {
        Ok<User> result = _userService.GetUser(id);

        return ConvertExempleTest(result);// you can create a Converter to change return from Result to Action automaticly
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

### Result

A abstract class base for Ok, Created and NoContent classes.
`Result` has properties base as Errors, StatusCode and IsSuccess.
`Result<TData>` is based on Result with data property

```csharp
public abstract class Result
{
    public IReadOnlyList<Error> Errors { get; init; }

    public HttpStatusCode StatusCode { get; init; }

    public bool IsSuccess => (int)StatusCode < 400;
}
```

### Result\<Data\>

`Result<TData>` is based on Result with data property

```csharp
public abstract class Result<TData> : Result
{
    private readonly TData? _data;

    public TData Data => IsSuccess ? _data! : throw new ArgumentException("http error has no data.");
}
```

## Star

if you enjoy, don't forget the ⭐ and install the package 😊.
