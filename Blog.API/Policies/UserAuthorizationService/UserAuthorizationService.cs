using APIBlog.Shared;
using Microsoft.AspNetCore.Authorization;

namespace APIBlog.Policies.Authorization;
public class UserAuthorizationService : IUserAuthorizationService
{
    private readonly IAuthorizationService _authorizationService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserAuthorizationService(IAuthorizationService authorizationService, IHttpContextAccessor httpContextAccessor)
    {
        _authorizationService = authorizationService;
        _httpContextAccessor = httpContextAccessor;
    }

    private async Task<Result> authorizeAsync(IAuthorizationRequirement requeriment)
    {
        var userLoged = _httpContextAccessor.HttpContext.User;

        var authorizationResult = await _authorizationService.AuthorizeAsync(userLoged, null, [requeriment]);

        if (!authorizationResult.Succeeded) return Result.Failure(Message.unauthorize_access, State.Forbidden);

        return Result.Succes();
    }

    public async Task<Result> AuthorizeUserAsync(int userId) => await authorizeAsync(new UserRequirement(userId));
    public async Task<Result> AuthorizeUserBlogAsync(int blogId) => await authorizeAsync(new BlogRequirement(blogId));
    public async Task<Result> AuthorizeUserPostUpdateAsync(int postId) => await authorizeAsync(new PostUpdateRequirement(postId));
    public async Task<Result> AuthorizeUserPostDeleteAsync(int postId) => await authorizeAsync(new PostDeleteRequirement(postId));
    public async Task<Result> AuthorizeUserCommentUpdateAsync(int commentId) => await authorizeAsync(new CommentUpdateRequeriment(commentId));
    public async Task<Result> AuthorizeUserCommentDeleteAsync(int commentId) => await authorizeAsync(new CommentDeleteRequeriment(commentId));

}