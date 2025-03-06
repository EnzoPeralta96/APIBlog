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
    private readonly IBlogRepository _blogRepository;
    private readonly IPostRepository _postRepository;

    private readonly ICommentRepository _commentRepository;
    public UserAuthorizationService(IAuthorizationService authorizationService, IHttpContextAccessor httpContextAccessor, IUserRepository userRepository, IBlogRepository blogRepository, IPostRepository postRepository, ICommentRepository commentRepository)
    {
        _authorizationService = authorizationService;
        _httpContextAccessor = httpContextAccessor;
        _userRepository = userRepository;
        _blogRepository = blogRepository;
        _postRepository = postRepository;
        _commentRepository = commentRepository;
    }

    public async Task<Result> AuthorizeUserAsync(int userId)
    {
        var requeriment = new UserRequirement(userId);

        var userLoged = _httpContextAccessor.HttpContext.User;

        var authorizationResult = await _authorizationService.AuthorizeAsync(userLoged, null, [requeriment]);

        if (!authorizationResult.Succeeded) return Result.Failure("Acceso no autorizado", State.Forbidden);

        return Result.Succes();
    }

    public async Task<Result> AuthorizeUserBlogAsync(int userId, int blogId)
    {
        var authorizationResult = await AuthorizeUserAsync(userId);

        if (!authorizationResult.IsSucces) return Result.Failure("Acceso no autorizado", State.Forbidden);

        bool isOwnerBlog = await _blogRepository.IsOwnerBlog(userId, blogId);

        if (!isOwnerBlog) return Result.Failure("El usuario indicado no es propietario del blog", State.BadRequest);

        return Result.Succes();
    }

    public async Task<Result> AuthorizeUserPostCreateAsync(int userId)
    {
        var authorizationResult = await AuthorizeUserAsync(userId);

        if (!authorizationResult.IsSucces) return Result.Failure("No se puede realizar un Post a nombre de otro usuario", State.Forbidden);

        var userLogedIsAdmin = _httpContextAccessor.HttpContext.User.IsInRole("admin");

        if (userLogedIsAdmin)
        {
            bool userExists = await _userRepository.ExistsAsync(userId);
            if (!userExists) return Result.Failure("El usuario indicado no existe", State.NotExist);
        }

        return Result.Succes();
    }

    public async Task<Result> AuthorizeUserPostRequestAsync(int userId, int postId)
    {
        var authorizationResult = await AuthorizeUserAsync(userId);

        if (!authorizationResult.IsSucces) return Result.Failure("Acceso no autorizado", State.Forbidden);

        bool IsOwnerPost = await _postRepository.IsOwnerPost(userId, postId);

        if (!IsOwnerPost) return Result.Failure("El usuario indicado no es propietario del Post", State.BadRequest);

        return Result.Succes();
    }

    public Task<Result> AuthorizeUserCommentAsync(int userId, int commentId)
    {
        throw new NotImplementedException();
    }
}