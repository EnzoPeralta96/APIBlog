using APIBlog.Repository;
using Microsoft.AspNetCore.Authorization;

namespace APIBlog.Policies;

public class CommentUpdateRequirementHandler : AuthorizationHandler<CommentUpdateRequeriment>
{
    private readonly ICommentRepository _commentRepository;
    public CommentUpdateRequirementHandler(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CommentUpdateRequeriment requirement)
    {
        int userLogedId = PoliciesHelper.GetUserLoggedId(context);

        if (userLogedId is 0) return Task.CompletedTask;

        bool userLoggedIsAdmin = PoliciesHelper.UserLoggedIsAdmin(context);

        bool userLoggedIsOwnerComment = _commentRepository.IsOwnerComment(userLogedId, requirement.CommentId);

        if (!userLoggedIsAdmin && !userLoggedIsOwnerComment) return Task.CompletedTask;

        context.Succeed(requirement);

        return Task.CompletedTask;
    }
}

