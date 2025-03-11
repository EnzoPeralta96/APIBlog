using Microsoft.AspNetCore.Authorization;

namespace APIBlog.Policies;

public class PostUpdateRequirement : IAuthorizationRequirement
{
    public PostUpdateRequirement(int postId)
    {
        PostId = postId;
    }
    public int PostId { get; }
}