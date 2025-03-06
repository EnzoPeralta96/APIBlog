
using Microsoft.AspNetCore.Authorization;

namespace APIBlog.Policies.Requirements;
public class UserRequirement : IAuthorizationRequirement
{
    public int UserId { get; }
    public UserRequirement(int userId)
    {
        UserId = userId;
    }
}