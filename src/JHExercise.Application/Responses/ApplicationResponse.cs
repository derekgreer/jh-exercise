namespace JHExercise.Application.Responses;

public class ApplicationResponse
{
    public ApplicationResponse(params string[] messages)
    {
        Messages = new List<string>();

        foreach (var message in messages)
            Messages.Add(message);
    }

    public ResponseStatusType Status { get; set; }

    public IList<string> Messages { get; set; }
}

public class ApplicationResponse<TData> : ApplicationResponse
{
    public ApplicationResponse(TData data, params string[] messages) : base(messages)
    {
        Data = data;
    }
    public TData Data { get; set; }
}