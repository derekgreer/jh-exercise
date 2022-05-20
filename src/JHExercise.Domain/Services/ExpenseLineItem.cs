using JHExercise.Domain.Records;

namespace JHExercise.Domain.Services;

class ExpenseLineItem
{
    public Department Department { get; set; }
    public int FiscalYear { get; set; }
    public double ExpensePercentage { get; set; }
    public double TotalExpensePercentage { get; set; }
    public double Revenue { get; set; }
    public double Expenses { get; set; }
}