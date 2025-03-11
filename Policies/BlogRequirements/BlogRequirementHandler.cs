
using APIBlog.Repository;
using Microsoft.AspNetCore.Authorization;

namespace APIBlog.Policies;

public class BlogRequirementHandler : AuthorizationHandler<BlogRequirement>
{
    private readonly IBlogRepository _blogRepository;
    public BlogRequirementHandler(IBlogRepository blogRepository)
    {
        _blogRepository = blogRepository;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, BlogRequirement requirement)
    {
        int userLogedId = PoliciesHelper.GetUserLoggedId(context);

        if (userLogedId is 0) return Task.CompletedTask;

        bool userLoggedIsAdmin = PoliciesHelper.UserLoggedIsAdmin(context);

        bool userLoggedIsOwnerBlog = _blogRepository.IsOwnerBlog(userLogedId, requirement.BlogId);

        if (!userLoggedIsAdmin && !userLoggedIsOwnerBlog ) return Task.CompletedTask;

        context.Succeed(requirement);

        return Task.CompletedTask;
    }
}