using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace APIBlog.Shared;

public class Result
{
    public bool IsSucces { get; }
    public string SuccesMessage { get; }
    public string ErrorMessage { get; }
    public State State { get; }

    private Result(bool isSucces, string message, State state)
    {
        IsSucces = isSucces;
        ErrorMessage = message;
        State = state;
    }

    private Result(bool isSucces, string succesMessage, string errorMesagge)
    {
        IsSucces = isSucces;
        SuccesMessage = succesMessage;
        ErrorMessage = errorMesagge;
    }
    
    public static Result Succes() => new Result(true,null,null);
    public static Result Succes(string succesMessage) => new Result(true, succesMessage,null);
    public static Result Failure(string errorMessage) => new Result(false,null,errorMessage);
    public static Result Failure(string errorMessage, State state) => new Result(false, errorMessage, state);

}