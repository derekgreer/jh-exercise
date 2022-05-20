using JHExercise.Domain.Records;

namespace JHExercise.Domain.Services;

public class YearOverYearExpenseLineItem
{
    public Department Department { get; set; }
    public int FiscalYear { get; set; }
    public double ExpensePercentage { get; set; }
    public double? YearOverYearExpensePercentageChange { get; set; }
}