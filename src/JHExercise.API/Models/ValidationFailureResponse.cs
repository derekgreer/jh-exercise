using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace JHExercise.API.Models;

public class ValidationFailureResponse
{
    public ValidationFailureResponse()
    {
    }

    public ValidationFailureResponse(ModelStateDictionary contextModelState)
    {
        ValidationErrors = new Dictionary<string, IEnumerable<string>>(contextModelState
            .Select(e =>
                new KeyValuePair<string, IEnumerable<string>>(e.Key, e.Value.Errors.Select(e => e.ErrorMessage))));
    }

    public IDictionary<string, IEnumerable<string>> ValidationErrors { get; set; }
}