using APIBlog.Repository;
using APIBlog.Services;
using Microsoft.AspNetCore.Authorization;

namespace APIBlog.Policies;

public class PostUpdateRequirementHandler : AuthorizationHandler<PostUpdateRequirement>
{
    private readonly IPostRepository _postRepository;

    public PostUpdateRequirementHandler(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PostUpdateRequirement requirement)
    {
        int userLogedId = PoliciesHelper.GetUserLoggedId(context);

        if (userLogedId is 0) return Task.CompletedTask;

        bool userLoggedIsAdmin = PoliciesHelper.UserLoggedIsAdmin(context);

        bool userLoggedIsOwnerPost = _postRepository.IsOwnerPost(userLogedId,requirement.PostId);

        if (!userLoggedIsAdmin && !userLoggedIsOwnerPost ) return Task.CompletedTask;

        context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
