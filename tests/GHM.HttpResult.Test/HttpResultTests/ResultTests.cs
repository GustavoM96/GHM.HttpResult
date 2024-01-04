using System.Net;
using GHM.HttpResult.Core;

namespace GHM.HttpResult.Test.HttpResultTests;

public class ResultTests
{
    private Created _createdResult => new();
    private Created<string> _createdDataResult => new("created correctly");
    private Created<string> _errorCreatedDataResult => new(_notFoundError);
    private Error _notFoundError => Error.NotFound("error title");
    private Error _conflictError => Error.Conflict("error title");

    [Fact]
    public void Test_Result_Contructor()
    {
        // Arrange
        Result successResult = new(new List<Error>(), HttpStatusCode.Created);
        Result successResult2 = new(HttpStatusCode.Created);
        Result errorResult = new(_notFoundError);

        // Act

        // Assert
        Assert.True(successResult.IsSuccess);
        Assert.True(successResult2.IsSuccess);
        Assert.False(errorResult.IsSuccess);
    }

    [Fact]
    public void Test_Successful_ShouldReturn_ResultWithHttpOk()
    {
        // Assert
        Assert.Equal(HttpStatusCode.OK, Result.Successful.StatusCode);
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
        var ex = Assert.ThrowsAny<Exception>(() => _createdResult.ToErrorResult());
        Assert.Equal("not found errors.", ex.Message);
    }

    [Fact]
    public void Test_ToErrorResult_When_FindErrors_ReturnErrorResult()
    {
        // Arrange
        Result error = new(_notFoundError);
        Result error2 = new(new List<Error>() { _notFoundError, _notFoundError }, HttpStatusCode.Created);

        // Act
        var errorResult = error.ToErrorResult();
        var errorResult2 = error2.ToErrorResult();

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, errorResult.StatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, errorResult2.StatusCode);
        Assert.Equal("error title", errorResult.Title);
        Assert.Equal("many error has occurred.", errorResult2.Title);
    }

    [Fact]
    public void Test_Match_When_Success_ShouldRun_OnSuccessFunc()
    {
        // Arrange
        static bool OnSuccess() => true;
        static bool OnError(ErrorResult error) => false;

        // Act
        var matchTest = _createdResult.Match(OnSuccess, OnError);

        // Assert
        Assert.True(matchTest);
    }

    [Fact]
    public void Test_Match_When_Error_ShouldRun_OnErrorFunc()
    {
        // Arrange
        static bool OnSuccess() => true;
        static bool OnError(ErrorResult error) => false;
        Result error = new(_notFoundError);

        // Act
        var matchTest = error.Match(OnSuccess, OnError);

        // Assert
        Assert.False(matchTest);
    }

    [Fact]
    public void Test_Tap_When_IsSuccessTrue_ShouldRun_Action()
    {
        // Arrange
        var dataCopy = "";
        void CopyData(string data) => dataCopy = data;

        // Act
        _createdDataResult.Tap(CopyData);

        // Assert
        Assert.Equal("created correctly", dataCopy);
    }

    [Fact]
    public void Test_Tap_When_IsSuccessFalse_ShouldNotRun_Action()
    {
        // Arrange
        var dataCopy = "";
        void CopyData(string data) => dataCopy = data;

        // Act
        _errorCreatedDataResult.Tap(CopyData);

        // Assert
        Assert.Equal("", dataCopy);
    }

    [Fact]
    public void Test_BindData_When_IsSuccessTrue_ShouldReturn_ActionResult()
    {
        // Arrange
        static string NewData(string data) => data + "_new";

        // Act
        var result = _createdDataResult.BindData(NewData);

        // Assert
        Assert.Equal("created correctly_new", result.Data);
    }

    [Fact]
    public void Test_BindData_When_IsSuccessFalse_ShouldNotReturn_ActionResult()
    {
        // Arrange
        static string NewData(string data) => data + "_new";

        // Act
        var result = _errorCreatedDataResult.BindData(NewData);

        // Assert
        Assert.Null(result.Data);
    }

    [Fact]
    public void Test_BindError_When_IsDataIsNotNull_ShouldReturn_ActionResult()
    {
        // Arrange
        Result CheckData(string data) => new(_conflictError);

        // Act
        var result = _createdDataResult.BindError(CheckData);

        // Assert
        Assert.Equal(_conflictError, result.Errors[0]);
    }

    [Fact]
    public void Test_BindError_When_IsDataIsNull_ShouldNotReturn_ActionResult()
    {
        // Arrange
        Result CheckData(string data) => new(_conflictError);

        // Act
        var result = _errorCreatedDataResult.BindError(CheckData);

        // Assert
        Assert.DoesNotContain(_conflictError, result.Errors);
    }

    [Fact]
    public void Test_BindError_Should_AddErrorByParamValue()
    {
        // Arrange
        // Act
        var successResult = _createdDataResult.BindError(false, _conflictError);
        var errorResult = _createdDataResult.BindError(true, _conflictError);

        // Assert
        Assert.Equal(_conflictError, errorResult.Errors[0]);
        Assert.DoesNotContain(_conflictError, successResult.Errors);
    }

    [Fact]
    public void Test_Map_When_DataIsNotNull_ShouldReturn_NewData()
    {
        // Arrange
        static string MapData(string data) => "new Data";

        // Act
        var successResult = _createdDataResult.Map(MapData);

        // Assert
        Assert.Equal("new Data", successResult.Data);
    }

    [Fact]
    public void Test_Map_When_DataIsNull_ShouldReturn_NewDataNull()
    {
        // Arrange
        static string MapData(string data) => "new Data";

        // Act
        var successResult = _errorCreatedDataResult.Map(MapData);

        // Assert
        Assert.Null(successResult.Data);
    }
}
