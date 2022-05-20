using JHExercise.API.Extensions;
using JHExercise.Application.Handlers;
using JHExercise.Application.Requests;
using Microsoft.AspNetCore.Mvc;

namespace JHExercise.API.Controllers;

[ApiController]
[Route("[controller]")]
public class DepartmentsController : ControllerBase
{
    [HttpGet]
    [Route("unprofitable")]
    public async Task<IActionResult> GetUnprofitableDepartments([FromQuery] GetUnprofitableDepartmentsRequestHandler.GetUnprofitableDepartmentsRequest request, [FromServices] GetUnprofitableDepartmentsRequestHandler handler)
    {
        return await handler.Handle(request).ToResult(r => new OkObjectResult(r), r => BadRequest());
    }
    
    [HttpGet]
    [Route("excessive-expenses")]
    public async Task<IActionResult> GetExcessiveExpenseDepartments([FromQuery] GetDepartmentsWithExcessiveExpensesRequest request, [FromServices] GetDepartmentsWithExcessiveExpensesRequestHandler handler)
    {
        return await handler.Handle(request).ToResult(r => new OkObjectResult(r), r => BadRequest());
    }
    
    [HttpGet]
    [Route("decreasing-expenses")]
    public async Task<IActionResult> GetDecreasingExpenseDepartments([FromQuery] GetDepartmentsWithDecreasingExpensesRequest request, [FromServices] GetDepartmentsWithDecreasingExpensesRequestHandler handler)
    {
        return await handler.Handle(request).ToResult(r => new OkObjectResult(r), r => BadRequest());
    }
}