using System.Net;

namespace GHM.HttpResult.Test.HttpResultTests;

public class HttpSuccessResultTests
{
    private static Created CreatedResult => new();
    private static Error NotFoundError => Error.NotFound("error title");
    private static Error ConflictError => Error.Conflict("error title");

    [Fact]
    public void Test_OKResult_Contructor()
    {
        // Arrange
        Ok successResult = Ok.Create;
        Ok errorResult = new(new Error[1] { NotFoundError });
        Ok errorResult2 = new(new List<Error>() { NotFoundError });
        Ok errorResult3 = NotFoundError;

        // Act

        // Assert
        Assert.True(successResult.IsValid);
        Assert.False(errorResult.IsValid);
        Assert.False(errorResult2.IsValid);
        Assert.False(errorResult3.IsValid);

        Assert.Equal(NotFoundError, errorResult.Errors[0]);
        Assert.Equal(NotFoundError, errorResult2.Errors[0]);
        Assert.Equal(NotFoundError, errorResult3.Errors[0]);
    }

    [Fact]
    public void Test_OKDataResult_Contructor()
    {
        // Arrange
        Ok<string> successResult = "dataTest";
        Ok<string> errorResult = new(new Error[1] { NotFoundError });
        Ok<string> errorResult2 = new(new List<Error>() { NotFoundError });
        Ok<string> errorResult3 = NotFoundError;

        // Act

        // Assert
        Assert.True(successResult.IsValid);
        Assert.False(errorResult.IsValid);
        Assert.False(errorResult2.IsValid);
        Assert.False(errorResult3.IsValid);

        Assert.Equal("dataTest", successResult.Data);
        Assert.Equal(NotFoundError, errorResult.Errors[0]);
        Assert.Equal(NotFoundError, errorResult2.Errors[0]);
        Assert.Equal(NotFoundError, errorResult3.Errors[0]);
    }

    [Fact]
    public void Test_CreatedResult_Contructor()
    {
        // Arrange
        Created successResult = Created.Create;
        Created errorResult = new(new Error[1] { NotFoundError });
        Created errorResult2 = new(new List<Error>() { NotFoundError });
        Created errorResult3 = NotFoundError;

        // Act

        // Assert
        Assert.True(successResult.IsValid);
        Assert.False(errorResult.IsValid);
        Assert.False(errorResult2.IsValid);
        Assert.False(errorResult3.IsValid);

        Assert.Equal(NotFoundError, errorResult.Errors[0]);
        Assert.Equal(NotFoundError, errorResult2.Errors[0]);
        Assert.Equal(NotFoundError, errorResult3.Errors[0]);
    }

    [Fact]
    public void Test_CreatedDataResult_Contructor()
    {
        // Arrange
        Created<string> successResult = "dataTest";
        Created<string> errorResult = new(new Error[1] { NotFoundError });
        Created<string> errorResult2 = new(new List<Error>() { NotFoundError });
        Created<string> errorResult3 = NotFoundError;

        // Act

        // Assert
        Assert.True(successResult.IsValid);
        Assert.False(errorResult.IsValid);
        Assert.False(errorResult2.IsValid);
        Assert.False(errorResult3.IsValid);

        Assert.Equal("dataTest", successResult.Data);
        Assert.Equal(NotFoundError, errorResult.Errors[0]);
        Assert.Equal(NotFoundError, errorResult2.Errors[0]);
        Assert.Equal(NotFoundError, errorResult3.Errors[0]);
    }

    [Fact]
    public void Test_NoContentResult_Contructor()
    {
        // Arrange
        NoContent successResult = NoContent.Create;
        NoContent errorResult = new(new Error[1] { NotFoundError });
        NoContent errorResult2 = new(new List<Error>() { NotFoundError });
        NoContent errorResult3 = NotFoundError;

        // Act

        // Assert
        Assert.True(successResult.IsValid);
        Assert.False(errorResult.IsValid);
        Assert.False(errorResult2.IsValid);
        Assert.False(errorResult3.IsValid);

        Assert.Equal(NotFoundError, errorResult.Errors[0]);
        Assert.Equal(NotFoundError, errorResult2.Errors[0]);
        Assert.Equal(NotFoundError, errorResult3.Errors[0]);
    }

    [Fact]
    public void Test_NoContentDataResult_Contructor()
    {
        // Arrange
        NoContent<string> successResult = "dataTest";
        NoContent<string> errorResult = new(new Error[1] { NotFoundError });
        NoContent<string> errorResult2 = new(new List<Error>() { NotFoundError });
        NoContent<string> errorResult3 = NotFoundError;

        // Act

        // Assert
        Assert.True(successResult.IsValid);
        Assert.False(errorResult.IsValid);
        Assert.False(errorResult2.IsValid);
        Assert.False(errorResult3.IsValid);

        Assert.Equal("dataTest", successResult.Data);
        Assert.Equal(NotFoundError, errorResult.Errors[0]);
        Assert.Equal(NotFoundError, errorResult2.Errors[0]);
        Assert.Equal(NotFoundError, errorResult3.Errors[0]);
    }

    [Fact]
    public void Test_Create_ShouldReturn_ResultWithHttp()
    {
        // Assert
        Assert.Equal(HttpStatusCode.OK, Ok.Create.StatusCode);
        Assert.Equal(HttpStatusCode.Created, Created.Create.StatusCode);
        Assert.Equal(HttpStatusCode.NoContent, NoContent.Create.StatusCode);
    }
}
