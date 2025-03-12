using Microsoft.AspNetCore.Authorization;

namespace APIBlog.Policies;

public class PostDeleteRequirement : IAuthorizationRequirement
{
    public PostDeleteRequirement(int postId)
    {
        PostId = postId;
    }
    public int PostId { get; }
}