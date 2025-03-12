using APIBlog.Repository;
using APIBlog.Services;
using Microsoft.AspNetCore.Authorization;

namespace APIBlog.Policies;

public class PostDeleteRequirementHandler : AuthorizationHandler<PostDeleteRequirement>
{
    private readonly IPostRepository _postRepository;

    public PostDeleteRequirementHandler(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PostDeleteRequirement requirement)
    {
        int userLogedId = PoliciesHelper.GetUserLoggedId(context);

        if (userLogedId is 0) return Task.CompletedTask;

        bool userLoggedIsAdmin = PoliciesHelper.UserLoggedIsAdmin(context);

        bool userLoggedIsOwnerBlog = _postRepository.IsOwnerBlogByPost(userLogedId,requirement.PostId);

        bool userLoggedIsOwnerPost = _postRepository.IsOwnerPost(userLogedId,requirement.PostId);

        if (!userLoggedIsAdmin && !userLoggedIsOwnerBlog && !userLoggedIsOwnerPost ) return Task.CompletedTask;

        context.Succeed(requirement);

        return Task.CompletedTask;
    }
}

