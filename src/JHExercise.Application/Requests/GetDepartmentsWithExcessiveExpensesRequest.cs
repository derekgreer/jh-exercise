namespace JHExercise.Application.Requests;

public class GetDepartmentsWithExcessiveExpensesRequest
{
    public double? PercentageThreshold { get; set; }
    public int? StartFiscalYear { get; set; }
    public int? DurationInYears { get; set; }
}