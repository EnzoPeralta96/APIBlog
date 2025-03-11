using APIBlog.Repository;
using Microsoft.AspNetCore.Authorization;

namespace APIBlog.Policies;

public class CommentDeleteRequirementHandler : AuthorizationHandler<CommentDeleteRequeriment>
{
    private readonly ICommentRepository _commentRepository;
    public CommentDeleteRequirementHandler(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CommentDeleteRequeriment requirement)
    {
        int userLogedId = PoliciesHelper.GetUserLoggedId(context);

        if (userLogedId is 0) return Task.CompletedTask;

        bool userLoggedIsAdmin = PoliciesHelper.UserLoggedIsAdmin(context);

        bool userLoggedIsOwnerComment = _commentRepository.IsOwnerComment(userLogedId, requirement.CommentId);

        bool userLoggedIsOwnerPost = _commentRepository.IsOwnerPostByComment(userLogedId,requirement.CommentId);

        if (!userLoggedIsAdmin && !userLoggedIsOwnerPost && !userLoggedIsOwnerComment) return Task.CompletedTask;

        context.Succeed(requirement);

        return Task.CompletedTask;
    }
}

