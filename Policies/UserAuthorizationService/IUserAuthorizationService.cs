using APIBlog.Shared;
namespace APIBlog.Policies.Authorization;
public interface IUserAuthorizationService
{
    Task<Result> AuthorizeUserAsync(int userId);
    Task<Result> AuthorizeUserAsync(int userId, int blogId);

}