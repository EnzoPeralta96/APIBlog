using APIBlog.Policies.Requirements;
using APIBlog.Repository;
using APIBlog.Shared;
using Microsoft.AspNetCore.Authorization;

namespace APIBlog.Policies.Authorization;
public class UserAuthorizationService : IUserAuthorizationService
{
    private readonly IAuthorizationService _authorizationService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserRepository _userRepository;

    public UserAuthorizationService(IAuthorizationService authorizationService, IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
    {
        _authorizationService = authorizationService;
        _httpContextAccessor = httpContextAccessor;
        _userRepository = userRepository;
    }

    public async Task<Result> AuthorizeUserAsync(int userId)
    {
        var requeriment = new UserRequirement(userId);

        var userLoged = _httpContextAccessor.HttpContext.User;

        var authorizationResult = await _authorizationService.AuthorizeAsync(userLoged, null, [requeriment]);

        if (!authorizationResult.Succeeded) return Result.Failure("Acceso no autorizado", State.Forbidden);

        return Result.Succes();
    }

    public async Task<Result> AuthorizeUserAsync(int userId, int blogId)
    {
        var authorizationResult = await AuthorizeUserAsync(userId);

        if (!authorizationResult.IsSucces) return Result.Failure("Acceso no autorizado", State.Forbidden);

        bool isOwnerBlog = await _userRepository.IsOwnerBlog(userId, blogId);

        if (!isOwnerBlog) return Result.Failure("El usuario indicado no es propietario del blog", State.BadRequest);

        return Result.Succes();
    }
}