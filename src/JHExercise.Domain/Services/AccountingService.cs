using JHExercise.Domain.Records;

namespace JHExercise.Domain.Services;

public class AccountingService : IAccountingService
{
    readonly IAccountingServiceClient _accountingServiceClient;

    public AccountingService(IAccountingServiceClient accountingServiceClient)
    {
        _accountingServiceClient = accountingServiceClient;
    }

    public async Task<IEnumerable<Department>> GetUnprofitableDepartments()
    {
        var records = await _accountingServiceClient.GetDepartmentAccountingRecords();
        return records.Where(r => r.FundsUsed >= r.FundsAvailable)
            .Select(r => new Department(r.DepartmentId, r.DepartmentName))
            .DistinctBy(r => r.ArgDepartmentId);
    }

    public async Task<IEnumerable<DepartmentExpense>> GetDepartmentsExceedingExpenses(double percentageThreshold, int startFiscalYear, int numberOfYears)
    {
        var departments = new List<DepartmentExpense>();
        var records = await _accountingServiceClient.GetDepartmentAccountingRecords();
        var recordGroups = records.GroupBy(r => r.Department);

        foreach (var group in recordGroups)
        {
            var departmentRecords = group.ToList();
            var set = departmentRecords.Where(r => int.Parse(r.FiscalYear) >= startFiscalYear)
                .Take(numberOfYears)
                .Select(r => new ExpenseLineItem
                {
                    FiscalYear = int.Parse(r.FiscalYear),
                    Revenue = r.FundsAvailable,
                    Expenses = r.FundsUsed,
                    ExpensePercentage = r.ExpensePercentage,
                    Department = r.Department
                })
                .ToList();

            foreach (var item in set)
            {
                item.TotalExpensePercentage = set.Where(r => r.FiscalYear <= item.FiscalYear).Sum(r => r.ExpensePercentage);
            }

            departments.AddRange(set.Where(b => b.TotalExpensePercentage >= percentageThreshold)
                .Select(r => new DepartmentExpense { FiscalYear = r.FiscalYear,  Department = group.Key, Revenue = r.Revenue, Expenses = r.Expenses }));
        }
        
        return departments;
    }

    public async Task<IEnumerable<DepartmentYearOverYearExpense>>  GetDepartmentsWithDecreasingExpenses(double percentageThreshold)
    {
        var departments = new List<DepartmentYearOverYearExpense>();
        var records = await _accountingServiceClient.GetDepartmentAccountingRecords();
        var recordGroups = records.GroupBy(r => r.Department);

        foreach (var group in recordGroups)
        {
            var departmentRecords = group.ToList();
            var set = departmentRecords
                .Select(r => new YearOverYearExpenseLineItem { FiscalYear = int.Parse(r.FiscalYear), ExpensePercentage = r.ExpensePercentage, Department = r.Department})
                .ToList();

            double? previousFiscalYearExpensesPercentage = null;
            
            foreach (var item in set)
            {
                if (previousFiscalYearExpensesPercentage.HasValue)
                {
                    item.YearOverYearExpensePercentageChange = item.ExpensePercentage - previousFiscalYearExpensesPercentage.Value;
                }

                previousFiscalYearExpensesPercentage = item.ExpensePercentage;
            }

            departments.AddRange(
                set.Where(b => b.YearOverYearExpensePercentageChange.HasValue && b.YearOverYearExpensePercentageChange < percentageThreshold)
                    .Select(r => new DepartmentYearOverYearExpense
                    {
                        Department = group.Key, 
                        FiscalYear = r.FiscalYear,
                        ExpenseChangeFromPreviousYear = r.YearOverYearExpensePercentageChange!.Value
                    })
            );
        }
        
        return departments;
    }
}

public class DepartmentExpense
{
    public Department Department { get; set; }
    public double Revenue { get; set; }
    public double Expenses { get; set; }
    public int FiscalYear { get; set; }
}