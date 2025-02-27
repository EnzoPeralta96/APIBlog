using System.Security.Claims;
using APIBlog.AuthorizationPoliciesSample.Policies.Requeriment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing.Constraints;

namespace APIBlog.AuthorizationPoliciesSample.Policies.Handlers;

public class UserRequirementHandler : AuthorizationHandler<UserRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserRequirement requirement)
    {

        var userIdClaim = context.User.FindFirst(cl => cl.Type == ClaimTypes.NameIdentifier);
        var userRolClaim = context.User.FindFirst(cl => cl.Type == ClaimTypes.Role);

        if (userIdClaim is null || userRolClaim is null) return Task.CompletedTask;

        bool isOwner = int.Parse((userIdClaim.Value)) == requirement.UserId;
        bool isAdmin = string.Equals(userRolClaim.Value, "admin", StringComparison.OrdinalIgnoreCase);
        
        if (isAdmin || isOwner)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}