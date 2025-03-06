using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace APIBlog.Policies.Requirements;
public class UserRequirementHandler : AuthorizationHandler<UserRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserRequirement requirement)
    {
        int userLogedId =   int.Parse(context.User.FindFirst(cl => cl.Type == ClaimTypes.NameIdentifier).Value);
        string userLogedRol = context.User.FindFirst(cl => cl.Type == ClaimTypes.Role).Value;

        if (userLogedId is 0 || userLogedRol is null) return Task.CompletedTask;

        bool isOwner = userLogedId == requirement.UserId;
        bool isAdmin = string.Equals(userLogedRol, "admin", StringComparison.OrdinalIgnoreCase);

        if (!isAdmin && !isOwner) return Task.CompletedTask;

        context.Succeed(requirement);

        return Task.CompletedTask;
    }
}