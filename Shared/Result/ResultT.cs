namespace APIBlog.Shared;

public class Result<T>
{
    public bool IsSucces { get; }
    public T Value { get; }
    public Message ErrorMessage { get; }
    public State State { get; }


    private Result(T value, bool isSucces)
    {
        Value = value;
        IsSucces = isSucces;
    }
    private Result(T value, bool isSucces, Message errorMessage, State state)
    {
        Value = value;
        IsSucces = isSucces;
        ErrorMessage = errorMessage;
        State = state;
    }

    public static Result<T> Succes(T value) => new Result<T>(value, true);
    public static Result<T> Failure(Message errorMessage, State state) => new Result<T>(default, false, errorMessage, state);
}