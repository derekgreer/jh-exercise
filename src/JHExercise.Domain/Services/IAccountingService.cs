using JHExercise.Domain.Records;

namespace JHExercise.Domain.Services;

public interface IAccountingService
{
    Task<IEnumerable<Department>> GetUnprofitableDepartments();
    Task<IEnumerable<DepartmentExpense>> GetDepartmentsExceedingExpenses(double percentageThreshold, int startFiscalYear, int numberOfYears);
    Task<IEnumerable<DepartmentYearOverYearExpense>>  GetDepartmentsWithDecreasingExpenses(double percentageThreshold);
}