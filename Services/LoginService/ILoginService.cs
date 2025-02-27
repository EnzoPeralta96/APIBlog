using APIBlog.Shared;
using APIBlog.ViewModels;

public interface ILoginService
{
    Task<Result> CreateAsync(UserLoginViewModel userCreateAccount, bool isAdmin = false);
    Task<Result<string>> LoginAsync(UserLoginViewModel userLogin);
}