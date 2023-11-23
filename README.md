# GHM.HttpResult

GHM.HttpResult is nuget packed with the aim of typing http result.

## Install Package

.NET CLI

```sh
dotnet add package GHM.HttpResult --version 1.0.0
```

Package Manager

```sh
NuGet\Install-Package GHM.HttpResult -Version 1.0.0
```

## Exemple

The exemple presents a demo how this lib works. Focused on implicit operator to easy all castings.

```csharp
public class UserService
{

    public HttpOk<User> GetUser(int id)
    {
        User user = _userRepo.GetUser(id);

        if(user is null)
        {
            return HttpError.NotFound($"not found user by id {id}");
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
        HttpOk<User> result = _userService.GetUser(id);
        return ConvertExempleTest(result);// you can create a Converting from Result to Action automaticly
    }

    [HttpGet]
    public IActionResult GetUser(int id)
    {
        HttpOk<User> result = _userService.GetUser(id);
        return result.Match(
            (data) => Ok(data),
            (errors) => BadRequest(errors)
        );// using a match pattern
    }
}

```

## Classes

## HttpResult

Is a abstract class base to HttpOk, HttpCreated and HttpNoContent.
`HttpResult` das the result base as Errors, StatusCode and Success property.
`HttpResult<TData>` is based on HttpResult with data property

```csharp
public abstract class HttpResult
{
    public IReadOnlyList<HttpError> Errors { get; init; }

    public HttpStatusCode StatusCode { get; init; }

    public bool IsSuccess => (int)StatusCode < 400;
}

public abstract class HttpResult<TData> : HttpResult
{
    private readonly TData? _data;

    public TData Data => IsSuccess ? _data! : throw new ArgumentException("http error has no data.");
}
```

## HttpSuccess

type of HttpSuccess:

- HttpOk
- HttpCreated
- HttpNoContent

```csharp
public class HttpOk<TData> : HttpResult<TData>{}
public class HttpOk : HttpResult<BasicResult> {}

public class HttpCreated<TData> : HttpResult<TData> {}
public class HttpCreated : HttpResult<BasicResult> {}

public class HttpNoContent : HttpResult{}

```

## HttpError

It is only one HttpError type with different HttpStatusCode:

- NotFound
- Forbidden
- BadRequest
- Unauthorized
- Conflict

```csharp
public class HttpError
{
    public string Title { get; init; }
    public HttpStatusCode StatusCode { get; init; }

    private HttpError(string title, HttpStatusCode httpStatusCode)
    {
        Title = title;
        StatusCode = httpStatusCode;
    }

    public static HttpError NotFound(string title) => new(title, HttpStatusCode.NotFound);

    public static HttpError Forbidden(string title) => new(title, HttpStatusCode.Forbidden);

    public static HttpError BadRequest(string title) => new(title, HttpStatusCode.BadRequest);

    public static HttpError Unauthorized(string title) => new(title, HttpStatusCode.Unauthorized);

    public static HttpError Conflict(string title) => new(title, HttpStatusCode.Conflict);
}

```

## Star

if you enjoy, don't forget the ⭐ and install the package 😊.
