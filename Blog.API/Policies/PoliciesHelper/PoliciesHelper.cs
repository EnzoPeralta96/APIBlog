using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace APIBlog.Policies;

public static class PoliciesHelper
{
    public static bool UserLoggedIsAdmin(AuthorizationHandlerContext context)
    {
        return context.User.IsInRole("admin");
    }

    public static int GetUserLoggedId(AuthorizationHandlerContext context)
    {
        return int.Parse(context.User.FindFirst(cl => cl.Type == ClaimTypes.NameIdentifier).Value);
    }
}