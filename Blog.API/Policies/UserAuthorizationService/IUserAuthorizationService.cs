using APIBlog.Shared;
namespace APIBlog.Policies.Authorization;
public interface IUserAuthorizationService
{
    Task<Result> AuthorizeUserAsync(int userId);
    Task<Result> AuthorizeUserBlogAsync(int blogId);
    Task<Result> AuthorizeUserPostUpdateAsync(int postId);
    Task<Result> AuthorizeUserPostDeleteAsync(int postId);

    Task<Result> AuthorizeUserCommentUpdateAsync(int commentId);

    Task<Result> AuthorizeUserCommentDeleteAsync(int commentId);
}