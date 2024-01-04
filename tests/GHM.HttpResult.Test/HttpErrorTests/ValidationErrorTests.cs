using System.Net;

namespace GHM.HttpResult.Test.HttpErrorTests;

public class ValidationErrorTests
{
    [Fact]
    public void Test_ValidationError_When_IsErrorTrue_ReturnErrorResult()
    {
        // Arrange
        Result notFound = new ValidationError(true).AsNotFound("error");
        Result forbidden = new ValidationError(true).AsForbidden("error");
        Result badRequest = new ValidationError(true).AsBadRequest("error");
        Result unauthorized = new ValidationError(true).AsUnauthorized("error");
        Result conflict = new ValidationError(true).AsConflict("error");

        // Act

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, notFound.StatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, forbidden.StatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, badRequest.StatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, unauthorized.StatusCode);
        Assert.Equal(HttpStatusCode.Conflict, conflict.StatusCode);
    }

    [Fact]
    public void Test_ValidationError_When_IsErrorfalse_ReturnSuccessfulResult()
    {
        // Arrangesucce
        Result result1 = new ValidationError(false).AsNotFound("error");
        Result result2 = new ValidationError(false).AsForbidden("error");
        Result result3 = new ValidationError(false).AsBadRequest("error");
        Result result4 = new ValidationError(false).AsUnauthorized("error");
        Result result5 = new ValidationError(false).AsConflict("error");

        // Act

        // Assert
        Assert.Equal(HttpStatusCode.OK, result1.StatusCode);
        Assert.Equal(HttpStatusCode.OK, result2.StatusCode);
        Assert.Equal(HttpStatusCode.OK, result3.StatusCode);
        Assert.Equal(HttpStatusCode.OK, result4.StatusCode);
        Assert.Equal(HttpStatusCode.OK, result5.StatusCode);
    }
}
