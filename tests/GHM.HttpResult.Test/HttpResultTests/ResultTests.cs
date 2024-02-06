using System.Net;

namespace GHM.HttpResult.Test.HttpResultTests;

public class ResultTests
{
    private static Result CreatedResult => new(HttpStatusCode.Created);
    private static Error NotFoundError => Error.NotFound("error title");
    private static Error ConflictError => Error.Conflict("error title");

    [Fact]
    public void Test_Ok_ShouldReturn_ResultWithHttp()
    {
        // Assert
        Assert.Equal(HttpStatusCode.OK, Result.Ok.StatusCode);
    }

    [Fact]
    public void Test_Validate_ShouldReturn_ValidationError()
    {
        // Assert
        Assert.True(Result.Validate(true).IsError);
        Assert.False(Result.Validate(false).IsError);
    }

    [Fact]
    public void Test_ToErrorResult_When_NotFoundErrors_ThrowException()
    {
        // Assert
        var ex = Assert.ThrowsAny<Exception>(() => CreatedResult.ToError());
        Assert.Equal("not found errors.", ex.Message);
    }

    [Fact]
    public void Test_ToErrorResult_When_FindErrors_ReturnErrorResult()
    {
        // Arrange
        Result error = new(NotFoundError);
        Result error2 = new(new List<Error>() { NotFoundError, NotFoundError });

        // Act
        var errorResult = error.ToError();
        var errorResult2 = error2.ToError();

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, errorResult.StatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, errorResult2.StatusCode);
        Assert.Equal("error title", errorResult.Message);
        Assert.Equal("many error has occurred.", errorResult2.Message);
    }

    [Fact]
    public void Test_ToSuccessDataResult_When_NotFoundErrors_ReturnResult()
    {
        // Arrange
        Result<string> successResult = "dataTest";
        Result successResult2 = Result.Ok;

        // Act
        var result = successResult.ToSuccess();
        var result2 = successResult2.ToSuccessResult();

        // Assert
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.Equal("dataTest", result.Data);

        Assert.Equal(HttpStatusCode.OK, successResult2.StatusCode);
    }

    [Fact]
    public void Test_AddError_Should_AddErrorAndSetStatus()
    {
        // Arrange
        Result successResult = Result.Ok;

        // Act
        successResult.AddError(NotFoundError);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, successResult.StatusCode);
    }

    [Fact]
    public void Test_AddErrors_Should_AddErrorAndSetStatus()
    {
        // Arrange
        Result successResult = Result.Ok;

        // Act
        successResult.AddErrors(new Error[2] { NotFoundError, ConflictError });

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, successResult.StatusCode);
    }

    [Fact]
    public void Test_Match_When_IsValid_ShouldRun_SuccessAction()
    {
        // Arrange
        Result<string> successResult = "success";
        Result successResult2 = Result.Ok;

        // Act
        var matchResult = successResult.Match((data) => data, (resultError) => "error");
        var matchResult2 = successResult2.Match((statusCode) => "ok", (resultError) => "error");

        // Assert
        Assert.Equal("success", matchResult);
        Assert.Equal("ok", matchResult2);
    }

    [Fact]
    public void Test_Match_When_IsNotValid_ShouldRun_SuccessAction()
    {
        // Arrange
        Result<string> errorResult = NotFoundError;
        Result errorResult2 = NotFoundError;

        // Act
        var matchResult = errorResult.Match((data) => data, (resultError) => "error");
        var matchResult2 = errorResult.Match((statusCode) => "ok", (resultError) => "error2");

        // Assert
        Assert.Equal("error", matchResult);
        Assert.Equal("error2", matchResult2);
    }
}
