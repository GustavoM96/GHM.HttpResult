using System.Net;

namespace GHM.HttpResult.Test.HttpErrorTests;

public class ErrorTests
{
    [Fact]
    public void Test_Error_Contructor()
    {
        // Arrange
        Error notFound = Error.NotFound("error");
        Error forbidden = Error.Forbidden("error");
        Error badRequest = Error.BadRequest("error");
        Error unauthorized = Error.Unauthorized("error");
        Error conflict = Error.Conflict("error");

        // Act

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, notFound.StatusCode);
        Assert.Equal(HttpStatusCode.Forbidden, forbidden.StatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, badRequest.StatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, unauthorized.StatusCode);
        Assert.Equal(HttpStatusCode.Conflict, conflict.StatusCode);
    }
}
