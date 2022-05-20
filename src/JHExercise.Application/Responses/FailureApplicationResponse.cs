namespace JHExercise.Application.Responses;

public class FailureApplicationResponse : ApplicationResponse
{
    public FailureApplicationResponse(string exceptionMessage) : base(exceptionMessage)
    {
        Status = ResponseStatusType.Failure;
    }
}