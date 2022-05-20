using JHExercise.Domain.Records;

namespace JHExercise.Domain.Services;

public class DepartmentYearOverYearExpense
{
    public Department Department { get; set; }
    public int FiscalYear { get; set; }
    public double ExpenseChangeFromPreviousYear { get; set; }
}