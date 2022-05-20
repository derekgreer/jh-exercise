using JHExercise.Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace JHExercise.API.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToResult<TResponse>(this TResponse response, Func<TResponse, IActionResult> success, Func<TResponse, IActionResult> error)
        where TResponse : ApplicationResponse
    {
        if (response.Status == ResponseStatusType.Success)
            return success(response);

        return error(response);
    }
    
    public async static Task<IActionResult> ToResult<TResponse>(this Task<TResponse> responseTask, Func<TResponse, IActionResult> success, Func<TResponse, IActionResult> error)
        where TResponse : ApplicationResponse
    {
        var response = await responseTask;
        
        if (response.Status == ResponseStatusType.Success)
            return success(response);

        return error(response);
    }
}