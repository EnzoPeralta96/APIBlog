using Microsoft.AspNetCore.Authorization;

namespace APIBlog.Policies.Requirements;
public class UserRequirementHandler : AuthorizationHandler<UserRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserRequirement requirement)
    {
        int userLogedId = HelperUserRequirements.GetUserLogedId(context);
        string userLogedRol = HelperUserRequirements.GetUserLogedRol(context);

        if (userLogedId is 0 || userLogedRol is null) return Task.CompletedTask;

        bool isOwner = userLogedId == requirement.UserId;
        bool isAdmin = string.Equals(userLogedRol, "admin", StringComparison.OrdinalIgnoreCase);

        if (!isAdmin && !isOwner) return Task.CompletedTask;

        context.Succeed(requirement);

        return Task.CompletedTask;
    }
}