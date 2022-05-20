using System.Net;
using System.Text.Json;
using JHExercise.Application.Exceptions;
using JHExercise.Application.Responses;

namespace JHExercise.API.Middleware;

public class ErrorHandlerMiddleware
{
    readonly ILogger<ErrorHandlerMiddleware> _logger;
    readonly RequestDelegate _next;
    
    public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            var responseMap = new Dictionary<Type, HttpStatusCode>
            {
                { typeof(NotFoundException), HttpStatusCode.NotFound },
                { typeof(BadRequestException), HttpStatusCode.BadRequest }
            };

            var status = responseMap.ContainsKey(e.GetType())
                ? responseMap[e.GetType()]
                : HttpStatusCode.InternalServerError;
            var failureResponse = new FailureApplicationResponse(e.Message);
            _logger.LogError(e, "An unhandled exception occurred.");

            var result = JsonSerializer.Serialize(failureResponse);
            response.StatusCode = (int)status;
            await response.WriteAsync(result);
        }
    }
}