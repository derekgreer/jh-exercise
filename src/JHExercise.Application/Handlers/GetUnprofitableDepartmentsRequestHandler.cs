using JHExercise.Application.Responses;
using JHExercise.Domain.Records;
using JHExercise.Domain.Services;

namespace JHExercise.Application.Handlers;

public class GetUnprofitableDepartmentsRequestHandler
{

    readonly IAccountingService _accountingService;

    public GetUnprofitableDepartmentsRequestHandler(IAccountingService accountingService)
    {
        _accountingService = accountingService;
    }

    public async Task<ApplicationResponse<IEnumerable<Department>>> Handle(GetUnprofitableDepartmentsRequest request)
    {
        IEnumerable<Department> departments = await _accountingService.GetUnprofitableDepartments();
        return new SuccessApplicationResponse<IEnumerable<Department>>(departments);
    }

    public class GetUnprofitableDepartmentsRequest
    {
    }
}