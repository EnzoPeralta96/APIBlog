
using Microsoft.AspNetCore.Authorization;

namespace APIBlog.Policies;
public class UserRequirement : IAuthorizationRequirement
{
    public int UserId { get; }
    public UserRequirement(int userId)
    {
        UserId = userId;
    }
}