
using Microsoft.AspNetCore.Authorization;

namespace APIBlog.AuthorizationPoliciesSample.Policies.Requeriment;
public class UserRequirement : IAuthorizationRequirement
{
    public int UserId { get; }
    public UserRequirement(int userId)
    {
        UserId = userId;
    }
}