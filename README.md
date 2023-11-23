# GHM.Result

GHM.Result is nuget packed with the aim of typing http result.

## Install Package

.NET CLI

```sh
dotnet add package GHM.Result --version 1.0.0
```

Package Manager

```sh
NuGet\Install-Package GHM.Result -Version 1.0.0
```

## Exemple

The exemple presents a demo of how this lib works. Focused on implicit operator to facilitate all castings.

```csharp
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
public class UserController : ControllerBase
{

    [HttpGet]
    public IActionResult GetUser(int id)
    {
        Ok<User> result = _userService.GetUser(id);
        return ConvertExempleTest(result);// you can create a Converting from Result to Action automaticly
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

## Result

Is a abstract class base to Ok, Created and NoContent.
`Result` has the result base as Errors, StatusCode and IsSuccess property.
`Result<TData>` is based on Result with data property

```csharp
public abstract class Result
{
    public IReadOnlyList<Error> Errors { get; init; }

    public HttpStatusCode StatusCode { get; init; }

    public bool IsSuccess => (int)StatusCode < 400;
}

public abstract class Result<TData> : Result
{
    private readonly TData? _data;

    public TData Data => IsSuccess ? _data! : throw new ArgumentException("http error has no data.");
}
```

## HttpSuccess

type of HttpSuccess:

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

## Error

It is only one Error type with different HttpStatusCode:

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

## Star

if you enjoy, don't forget the ⭐ and install the package 😊.
