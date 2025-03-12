using System.Security.Claims;
using APIBlog.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Razor;

namespace APIBlog.Policies;
public class UserRequirementHandler : AuthorizationHandler<UserRequirement>
{
    private readonly IUserRepository _userRepository;
    public UserRequirementHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserRequirement requirement)
    {
        int userLogedId = PoliciesHelper.GetUserLoggedId(context);

        if (userLogedId is 0) return Task.CompletedTask;

        bool isAdmin = PoliciesHelper.UserLoggedIsAdmin(context);

        bool isOwner = userLogedId == requirement.UserId;

        if (!isAdmin && !isOwner) return Task.CompletedTask;
        
        bool userExist = _userRepository.Exists(requirement.UserId);

        if(isAdmin && !userExist) return Task.CompletedTask;
        
        context.Succeed(requirement);

        return Task.CompletedTask;
    }
}