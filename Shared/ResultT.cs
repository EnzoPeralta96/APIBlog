namespace APIBlog.Shared;

public class Result<T>
{
    public bool IsSucces { get; }
    public T Value { get; }
    public string ErrorMessage { get; }
    //public State State { get; }
    private Result(T value, bool isSucces, string errorMessage/*, State state*/)
    {
        Value = value;
        IsSucces = isSucces;
        ErrorMessage = errorMessage;
        //State = state;
    }
    public static Result<T> Succes(T value/*, State state*/) => new Result<T>(value, true, null/*, state*/);
    public static Result<T> Failure(string errorMessage/*, State state*/) => new Result<T>(default, false, errorMessage/*, state*/);
}