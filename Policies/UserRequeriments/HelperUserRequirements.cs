using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
namespace APIBlog.Policies.Requirements;
public static class HelperUserRequirements
{
    public static string GetUserLogedRol(AuthorizationHandlerContext context)
    {
        return context.User.FindFirst(cl => cl.Type == ClaimTypes.Role).Value;
    }

    public static int GetUserLogedId(AuthorizationHandlerContext context)
    {
        return int.Parse(context.User.FindFirst(cl => cl.Type == ClaimTypes.NameIdentifier).Value);
    }
}