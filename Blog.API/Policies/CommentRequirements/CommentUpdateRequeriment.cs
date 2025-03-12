using Microsoft.AspNetCore.Authorization;

namespace APIBlog.Policies;

public class CommentUpdateRequeriment : IAuthorizationRequirement
{
    public CommentUpdateRequeriment(int commentId)
    {
        CommentId = commentId;
    }

    public int CommentId { get; }
}