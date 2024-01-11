namespace GHM.HttpResult;

public static class ResultExtensions
{
    public static async Task<Ok<TData>> BindErrorAsync<TData>(this Task<Ok<TData>> okTask, Func<TData, Task<Result>> action)
    {
        var ok = await okTask;
        return await ok.BindErrorAsync(action);
    }

    public static async Task<Ok<TData>> BindDataAsync<TData>(this Task<Ok<TData>> okTask, Func<TData, Task<TData>> action)
    {
        var ok = await okTask;
        return await ok.BindDataAsync(action);
    }

    public static async Task<Ok<TData>> TapAsync<TData>(this Task<Ok<TData>> okTask, Func<TData, Task> action)
    {
        var ok = await okTask;
        return await ok.TapAsync(action);
    }

    public static async Task<Created<TData>> BindErrorAsync<TData>(
        this Task<Created<TData>> createdTask,
        Func<TData, Task<Result>> action
    )
    {
        var created = await createdTask;
        return await created.BindErrorAsync(action);
    }

    public static async Task<Created<TData>> BindDataAsync<TData>(
        this Task<Created<TData>> createdTask,
        Func<TData, Task<TData>> action
    )
    {
        var created = await createdTask;
        return await created.BindDataAsync(action);
    }

    public static async Task<Created<TData>> TapAsync<TData>(this Task<Created<TData>> createdTask, Func<TData, Task> action)
    {
        var created = await createdTask;
        return await created.TapAsync(action);
    }

    public static async Task<NoContent<TData>> BindErrorAsync<TData>(
        this Task<NoContent<TData>> noContentTask,
        Func<TData, Task<Result>> action
    )
    {
        var noContent = await noContentTask;
        return await noContent.BindErrorAsync(action);
    }

    public static async Task<NoContent<TData>> BindDataAsync<TData>(
        this Task<NoContent<TData>> noContentTask,
        Func<TData, Task<TData>> action
    )
    {
        var noContent = await noContentTask;
        return await noContent.BindDataAsync(action);
    }

    public static async Task<NoContent<TData>> TapAsync<TData>(
        this Task<NoContent<TData>> noContentTask,
        Func<TData, Task> action
    )
    {
        var noContent = await noContentTask;
        return await noContent.TapAsync(action);
    }
}
