namespace APIBlog.Shared;
public enum State
{
    Default,
    Deleted,
    NotExist,
    NameInUse,
    PasswordsDifferents,
    InternalServerError,
    Unauthorized,
    Forbidden,
    BadRequest
}