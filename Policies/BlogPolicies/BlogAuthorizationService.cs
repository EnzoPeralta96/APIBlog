using APIBlog.AuthorizationPoliciesSample.Policies.Requeriment;
using APIBlog.Repository;
using APIBlog.Shared;
using APIBlog.ViewModels;
using Microsoft.AspNetCore.Authorization;

public class BlogAuthorizationService
{
    private IAuthorizationService _authorizationService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserRepository _userRepository;

    public BlogAuthorizationService(IAuthorizationService authorizationService, IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
    {
        _authorizationService = authorizationService;
        _httpContextAccessor = httpContextAccessor;
        _userRepository = userRepository;
    }

    public async Task<Result> AuthorizeAsync(int ownerBlogId)
    {
        var requeriment = new UserRequirement(ownerBlogId);

        var User = _httpContextAccessor.HttpContext.User;

        var authorizationResult = await _authorizationService.AuthorizeAsync(User, null, [requeriment]);

        /*
        - No es admin 
        - El usuario logueado no es el dueño del blog
        - Por lo tanto es un usuario que quiere crear un blog para otro usuario (No cumplo con la regla)
        */
        if (!authorizationResult.Succeeded)  return Result.Failure("Acción no autorizada", State.Forbidden);
      
        //El administrador quiere crear/modificar un blog para/de un usuario/
        bool userExists = await _userRepository.ExistsAsync(ownerBlogId);

        if (User.IsInRole("admin") && !userExists) return Result.Failure("El usuario indicado no existe", State.NotExist);

        return Result.Succes();
    }

    public async Task<Result> AuthorizeAsync(int ownerBlogId, int blogId)
    {
        var requeriment = new UserRequirement(ownerBlogId);

        var User = _httpContextAccessor.HttpContext.User;

        var authorizationResult = await _authorizationService.AuthorizeAsync(User, null, [requeriment]);

        /*
        - No es admin 
        - El usuario logueado no es el dueño del blog
        - Por lo tanto es un usuario que quiere crear un blog para otro usuario (No cumplo con la regla)
        */
        if (!authorizationResult.Succeeded) return Result.Failure("Acción no autorizada", State.Forbidden);

        bool userExists = await _userRepository.ExistsAsync(ownerBlogId);

        if (User.IsInRole("admin") && !userExists) return Result.Failure("El usuario indicado no existe", State.NotExist);

        bool userIsOwner = await _userRepository.IsOwnerBlog(ownerBlogId, blogId);

        if (!userIsOwner) return Result.Failure("El usuario indicado no es propietario del blog", State.Forbidden);

        //El administrador quiere crear/modificar un blog para/de un usuario/


        return Result.Succes();
    }





}
