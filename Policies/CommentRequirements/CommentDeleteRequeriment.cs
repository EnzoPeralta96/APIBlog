using Microsoft.AspNetCore.Authorization;

namespace APIBlog.Policies;

public class CommentDeleteRequeriment : IAuthorizationRequirement
{
    public CommentDeleteRequeriment(int commentId)
    {
        CommentId = commentId;
    }

    public int CommentId { get; }
}