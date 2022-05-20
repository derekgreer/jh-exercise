using JHExercise.Application.Requests;
using JHExercise.Application.Responses;
using JHExercise.Domain.Services;

namespace JHExercise.Application.Handlers;

public class GetDepartmentsWithDecreasingExpensesRequestHandler
{
    readonly IAccountingService _accountingService;

    public GetDepartmentsWithDecreasingExpensesRequestHandler(IAccountingService accountingService)
    {
        _accountingService = accountingService;
    }
    public async Task<ApplicationResponse<IEnumerable<DepartmentYearOverYearExpense>>> Handle(GetDepartmentsWithDecreasingExpensesRequest request)
    {
        var departments = await _accountingService.GetDepartmentsWithDecreasingExpenses(request.PercentageThreshold!.Value);
        return new SuccessApplicationResponse<IEnumerable<DepartmentYearOverYearExpense>>(departments);
    }
}