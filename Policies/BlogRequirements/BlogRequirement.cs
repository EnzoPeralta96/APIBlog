using Microsoft.AspNetCore.Authorization;

namespace APIBlog.Policies;

public class BlogRequirement : IAuthorizationRequirement
{
    public BlogRequirement(int blogId)
    {
        BlogId = blogId;
    }

    public int BlogId {get;}
}