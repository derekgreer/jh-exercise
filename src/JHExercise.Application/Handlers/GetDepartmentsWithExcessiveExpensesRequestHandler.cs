using JHExercise.Application.Requests;
using JHExercise.Application.Responses;
using JHExercise.Domain.Services;

namespace JHExercise.Application.Handlers;

public class GetDepartmentsWithExcessiveExpensesRequestHandler
{
    readonly IAccountingService _accountingService;

    public GetDepartmentsWithExcessiveExpensesRequestHandler(IAccountingService accountingService)
    {
        _accountingService = accountingService;
    }

    public async Task<ApplicationResponse<IEnumerable<DepartmentExpense>>> Handle(
        GetDepartmentsWithExcessiveExpensesRequest request)
    {
        var departments = await _accountingService.GetDepartmentsExceedingExpenses(request.PercentageThreshold!.Value,
            request.StartFiscalYear!.Value, request.DurationInYears!.Value);
        return new SuccessApplicationResponse<IEnumerable<DepartmentExpense>>(departments);
    }
}