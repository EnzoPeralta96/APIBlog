namespace APIBlog.Shared;
public enum State
{
    NotExist,
    NameInUse,
    PasswordsDifferents,
    InternalServerError,
    Forbidden,
    BadRequest
}