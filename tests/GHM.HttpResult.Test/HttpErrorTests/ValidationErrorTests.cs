using System.Net;

namespace GHM.HttpResult.Test.HttpErrorTests;

public class ValidationErrorTests
{
    [Fact]
    public void Test_ValidationError_When_IsErrorTrue_ReturnErrorResult()
    {
        // Arrange
        Result notFound = Result.Validate(true).AsNotFound("error1");
        Result forbidden = Result.Validate(true).AsForbidden("error2");
        Result badRequest = Result.Validate(true).AsBadRequest("error3");
        Result unauthorized = Result.Validate(true).AsUnauthorized("error4");
        Result conflict = Result.Validate(true).AsConflict("error5");

        // Act

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, notFound.StatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, forbidden.StatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, badRequest.StatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, unauthorized.StatusCode);
        Assert.Equal(HttpStatusCode.Conflict, conflict.StatusCode);

        Assert.Equal("error1", notFound.Message);
        Assert.Equal("error2", forbidden.Message);
        Assert.Equal("error3", badRequest.Message);
        Assert.Equal("error4", unauthorized.Message);
        Assert.Equal("error5", conflict.Message);
    }

    [Fact]
    public void Test_ValidationError_When_IsErrorfalse_ReturnSuccessfulResult()
    {
        // Arrangesucce
        Result result1 = new ValidationResult(false).AsNotFound("error");
        Result result2 = new ValidationResult(false).AsForbidden("error");
        Result result3 = new ValidationResult(false).AsBadRequest("error");
        Result result4 = new ValidationResult(false).AsUnauthorized("error");
        Result result5 = new ValidationResult(false).AsConflict("error");

        // Act

        // Assert
        Assert.Equal(HttpStatusCode.OK, result1.StatusCode);
        Assert.Equal(HttpStatusCode.OK, result2.StatusCode);
        Assert.Equal(HttpStatusCode.OK, result3.StatusCode);
        Assert.Equal(HttpStatusCode.OK, result4.StatusCode);
        Assert.Equal(HttpStatusCode.OK, result5.StatusCode);
    }
}
