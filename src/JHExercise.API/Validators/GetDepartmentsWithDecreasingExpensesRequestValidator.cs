using FluentValidation;
using JHExercise.Application.Requests;

namespace JHExercise.API.Validators;

public class GetDepartmentsWithDecreasingExpensesRequestValidator : AbstractValidator<GetDepartmentsWithDecreasingExpensesRequest>
{
    public GetDepartmentsWithDecreasingExpensesRequestValidator()
    {
        RuleFor(x => x.PercentageThreshold).NotEmpty();
    }
}