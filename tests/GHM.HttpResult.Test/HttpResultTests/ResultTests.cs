using System.Net;

namespace GHM.HttpResult.Test.HttpResultTests;

public class ResultTests
{
    private static Created CreatedResult => new();
    private static Created<string> CreatedDataResult => new("created correctly");
    private static Created<string> ErrorCreatedDataResult => new(NotFoundError);
    private static Error NotFoundError => Error.NotFound("error title");
    private static Error ConflictError => Error.Conflict("error title");

    [Fact]
    public void Test_Result_Contructor()
    {
        // Arrange
        Result successResult = new(new List<Error>(), HttpStatusCode.Created);
        Result successResult2 = new(HttpStatusCode.Created);
        Result errorResult = new(NotFoundError);

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
        var ex = Assert.ThrowsAny<Exception>(() => CreatedResult.ToErrorResult());
        Assert.Equal("not found errors.", ex.Message);
    }

    [Fact]
    public void Test_ToErrorResult_When_FindErrors_ReturnErrorResult()
    {
        // Arrange
        Result error = new(NotFoundError);
        Result error2 = new(new List<Error>() { NotFoundError, NotFoundError }, HttpStatusCode.Created);

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
        var matchTest = CreatedResult.Match(OnSuccess, OnError);

        // Assert
        Assert.True(matchTest);
    }

    [Fact]
    public void Test_Match_When_Error_ShouldRun_OnErrorFunc()
    {
        // Arrange
        static bool OnSuccess() => true;
        static bool OnError(ErrorResult error) => false;
        Result error = new(NotFoundError);

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
        CreatedDataResult.Tap(CopyData);

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
        ErrorCreatedDataResult.Tap(CopyData);

        // Assert
        Assert.Equal("", dataCopy);
    }

    [Fact]
    public async Task Test_TapAsync_When_IsSuccessTrue_ShouldRun_Action()
    {
        // Arrange
        var dataCopy = "";
        Task CopyData(string data) => Task.FromResult(dataCopy = data);

        // Act
        await CreatedDataResult.TapAsync(CopyData);

        // Assert
        Assert.Equal("created correctly", dataCopy);
    }

    [Fact]
    public async Task Test_TapAsync_When_IsSuccessFalse_ShouldNotRun_Action()
    {
        // Arrange
        var dataCopy = "";
        Task CopyData(string data) => Task.FromResult(dataCopy = data);

        // Act
        await ErrorCreatedDataResult.TapAsync(CopyData);

        // Assert
        Assert.Equal("", dataCopy);
    }

    [Fact]
    public void Test_BindData_When_IsSuccessTrue_ShouldReturn_ActionResult()
    {
        // Arrange
        static string NewData(string data) => data + "_new";

        // Act
        var result = CreatedDataResult.BindData(NewData);

        // Assert
        Assert.Equal("created correctly_new", result.Data);
    }

    [Fact]
    public void Test_BindData_When_IsSuccessFalse_ShouldNotReturn_ActionResult()
    {
        // Arrange
        static string NewData(string data) => data + "_new";

        // Act
        var result = ErrorCreatedDataResult.BindData(NewData);

        // Assert
        Assert.Null(result.Data);
    }

    [Fact]
    public async Task Test_BindDataAsync_When_IsSuccessTrue_ShouldReturn_ActionResult()
    {
        // Arrange
        static Task<string> NewData(string data) => Task.FromResult(data + "_new");

        // Act
        var result = await CreatedDataResult.BindDataAsync(NewData);

        // Assert
        Assert.Equal("created correctly_new", result.Data);
    }

    [Fact]
    public async Task Test_BindDataAsync_When_IsSuccessFalse_ShouldNotReturn_ActionResult()
    {
        // Arrange
        static Task<string> NewData(string data) => Task.FromResult(data + "_new");

        // Act
        var result = await ErrorCreatedDataResult.BindDataAsync(NewData);

        // Assert
        Assert.Null(result.Data);
    }

    [Fact]
    public void Test_BindError_When_IsDataIsNotNull_ShouldReturn_ActionResult()
    {
        // Arrange
        Result CheckData(string data) => new(ConflictError);

        // Act
        var result = CreatedDataResult.BindError(CheckData);

        // Assert
        Assert.Equal(ConflictError, result.Errors[0]);
    }

    [Fact]
    public void Test_BindError_When_IsDataIsNull_ShouldNotReturn_ActionResult()
    {
        // Arrange
        Result CheckData(string data) => new(ConflictError);

        // Act
        var result = ErrorCreatedDataResult.BindError(CheckData);

        // Assert
        Assert.DoesNotContain(ConflictError, result.Errors);
    }

    [Fact]
    public async Task Test_BindErrorAsync_Should_AddErrorByParamValue()
    {
        // Arrange
        Task<Result> GetErrorResult(string data) => Task.FromResult(new Result(ConflictError));
        Task<Result> GetSuccessResult(string data) => Task.FromResult(Result.Successful);

        // Act
        var successResult = await CreatedDataResult.BindErrorAsync(GetSuccessResult);
        var errorResult = await CreatedDataResult.BindErrorAsync(GetErrorResult);

        // Assert
        Assert.Equal(ConflictError, errorResult.Errors[0]);
        Assert.DoesNotContain(ConflictError, successResult.Errors);
    }

    [Fact]
    public void Test_Map_When_DataIsNotNull_ShouldReturn_NewData()
    {
        // Arrange
        static string MapData(string data) => "new Data";

        // Act
        var successResult = CreatedDataResult.Map(MapData);

        // Assert
        Assert.Equal("new Data", successResult.Data);
    }

    [Fact]
    public void Test_Map_When_DataIsNull_ShouldReturn_NewDataNull()
    {
        // Arrange
        static string MapData(string data) => "new Data";

        // Act
        var successResult = ErrorCreatedDataResult.Map(MapData);

        // Assert
        Assert.Null(successResult.Data);
    }
}
