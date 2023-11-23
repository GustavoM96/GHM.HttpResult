using GHM.HttpResult.Result;
using Microsoft.AspNetCore.Mvc;

namespace GHM.HttpResult.Extentions;

public static class HttpResultExtensions
{
    public static ActionResult ToActionResult<TData>(this HttpResult<TData> result)
    {
        if (result.IsSuccess)
        {
            var success = result.ToSuccessResult();
            return new ObjectResult(success) { StatusCode = (int)success.StatusCode };
        }

        var error = result.ToErrorResult();
        return new ObjectResult(error) { StatusCode = (int)error.StatusCode };
    }
}
