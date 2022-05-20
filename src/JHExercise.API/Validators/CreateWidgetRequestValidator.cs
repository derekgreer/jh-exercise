using FluentValidation;
using JHExercise.Application.Requests;

namespace JHExercise.API.Validators;

public class GetDepartmentsWithExcessiveExpensesRequestValidator : AbstractValidator<GetDepartmentsWithExcessiveExpensesRequest>
{
    public GetDepartmentsWithExcessiveExpensesRequestValidator()
    {
        RuleFor(x => x.PercentageThreshold).NotEmpty();
        RuleFor(x => x.StartFiscalYear).NotEmpty();
        RuleFor(x => x.DurationInYears).NotEmpty();
    }
}