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
        if (userIdClaim is null ) return Task.CompletedTask;

        int userIdValue = int.Parse((userIdClaim.Value));
        if (userIdValue == requirement.UserId)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}