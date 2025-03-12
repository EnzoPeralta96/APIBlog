namespace APIBlog.Shared;
public class Result
{
    public bool IsSucces { get; }
    public Message SuccesMessage { get; }
    public Message ErrorMessage { get; }
    public State State { get; }

    private Result(bool isSucces, Message message, State state)
    {
        IsSucces = isSucces;
        ErrorMessage = message;
        State = state;
    }
    private Result(bool isSucces, Message succesMessage)
    {
        IsSucces = isSucces;
        SuccesMessage = succesMessage;
    }

    private Result(bool isSucces)
    {
        IsSucces = isSucces;
    }

    public static Result Succes() => new Result(true);
    public static Result Succes(Message succesMessage) => new Result(true, succesMessage);
    public static Result Failure(Message errorMessage, State state) => new Result(false, errorMessage, state);
}