namespace JHExercise.Application.Responses;

public class SuccessApplicationResponse<TData> : ApplicationResponse<TData>
{
    public SuccessApplicationResponse(TData data, params string[] messages) : base(data, messages)
    {
        Data = data;
        Status = ResponseStatusType.Success;
    }

    public SuccessApplicationResponse(params string[] messages) : this(default(TData), messages)
    {

    }
}

public class SuccessApplicationResponse : ApplicationResponse
{
    public SuccessApplicationResponse(params string[] messages) : base(messages)
    {
        Status = ResponseStatusType.Success;
    }
}