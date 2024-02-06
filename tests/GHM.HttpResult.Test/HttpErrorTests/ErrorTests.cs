using System.Net;

namespace GHM.HttpResult.Test.HttpErrorTests;

public class ErrorTests
{
    [Fact]
    public void Test_Error_Contructor()
    {
        // Arrange
        Error notFound = Error.NotFound("error1");
        Error forbidden = Error.Forbidden("error2");
        Error badRequest = Error.BadRequest("error3");
        Error unauthorized = Error.Unauthorized("error4");
        Error conflict = Error.Conflict("error5");

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
}
