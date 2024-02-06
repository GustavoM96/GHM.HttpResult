using System.Net;

namespace GHM.HttpResult.Test.HttpResultTests;

public class ResultTests
{
    private static Created CreatedResult => new();
    private static Error NotFoundError => Error.NotFound("error title");
    private static Error ConflictError => Error.Conflict("error title");

    [Fact]
    public void Test_Result_Contructor()
    {
        // Arrange
        Result successResult = new(new List<Error>());
        Result successResult2 = new(HttpStatusCode.Created);
        Result errorResult = new(NotFoundError);

        // Act

        // Assert
        Assert.True(successResult.IsValid);
        Assert.True(successResult2.IsValid);
        Assert.False(errorResult.IsValid);
    }

    [Fact]
    public void Test_Successful_ShouldReturn_ResultWithHttp()
    {
        // Assert
        Assert.Equal(HttpStatusCode.OK, Result.Ok.StatusCode);
        Assert.Equal(HttpStatusCode.OK, Ok.Create.StatusCode);
        Assert.Equal(HttpStatusCode.Created, Created.Create.StatusCode);
        Assert.Equal(HttpStatusCode.NoContent, NoContent.Create.StatusCode);
    }

    [Fact]
    public void Test_Validate_When_SetTrue_ShouldReturn_ValidationError()
    {
        // Assert
        Assert.True(Result.Validate(true).IsError);
    }

    [Fact]
    public void Test_ToErrorResult_When_NotFoundErrors_ThrowException()
    {
        // Assert
        var ex = Assert.ThrowsAny<Exception>(() => CreatedResult.ToErrorResult());
        Assert.Equal("not found errors.", ex.Message);
    }

    [Fact]
    public void Test_ToErrorResult_When_FindErrors_ReturnErrorResult()
    {
        // Arrange
        Result error = new(NotFoundError);
        Result error2 = new(new List<Error>() { NotFoundError, NotFoundError });

        // Act
        var errorResult = error.ToErrorResult();
        var errorResult2 = error2.ToErrorResult();

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, errorResult.StatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, errorResult2.StatusCode);
        Assert.Equal("error title", errorResult.Message);
        Assert.Equal("many error has occurred.", errorResult2.Message);
    }

    [Fact]
    public void Test_AddError_Should_AddErrorAndSetStatus()
    {
        // Arrange
        Ok<string> ok = "success";

        // Act
        ok.AddError(NotFoundError);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, ok.StatusCode);
    }

    [Fact]
    public void Test_AddErrors_Should_AddErrorAndSetStatus()
    {
        // Arrange
        Ok<string> ok = "success";

        // Act
        ok.AddError(NotFoundError);
        ok.AddError(ConflictError);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, ok.StatusCode);
    }
}
