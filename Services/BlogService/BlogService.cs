using APIBlog.Models;
using APIBlog.Policies.Authorization;
using APIBlog.Repository;
using APIBlog.Shared;
using APIBlog.ViewModels;
using AutoMapper;

namespace APIBlog.Services;
public class BlogService : IBlogService
{
    private readonly ILogger _logger;
    private readonly IUserAuthorizationService _userAuthorizationService;
    private readonly IBlogRepository _blogRepository;

    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    public BlogService(IUserAuthorizationService userAuthorizationService, IBlogRepository blogRepository, IUserRepository userRepository, IMapper mapper, ILogger<BlogService> logger)
    {

        _blogRepository = blogRepository;
        _userRepository = userRepository;
        _userAuthorizationService = userAuthorizationService;
        _mapper = mapper;
        _logger = logger;
    }
    public async Task<Result<List<BlogViewModel>>> BlogsAsync(int userId)
    {
        try
        {
            var userExists = await _userRepository.ExistsAsync(userId);

            if (!userExists) return Result<List<BlogViewModel>>.Failure(Message.user_not_exist, State.NotExist);

            var blogs = await _blogRepository.BlogsAsync(userId);

            var blogsViewModel = _mapper.Map<List<BlogViewModel>>(blogs);

            return Result<List<BlogViewModel>>.Succes(blogsViewModel);
        }
        catch (Exception ex)
        {
              _logger.LogError("Error inesperado: " + ex.ToString());
            return Result<List<BlogViewModel>>.Failure(Message.unexpected_error, State.InternalServerError);
        }
    }

    public async Task<Result<BlogViewModel>> BlogAsync(int id)
    {
        try
        {
            var blog = await _blogRepository.GetBlogAsync(id);

            if (blog is null) return Result<BlogViewModel>.Failure(Message.blog_not_exist, State.NotExist);

            var blogViewModel = _mapper.Map<BlogViewModel>(blog);

            return Result<BlogViewModel>.Succes(blogViewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error inesperado: " + ex.ToString());
            return Result<BlogViewModel>.Failure(Message.unexpected_error, State.InternalServerError);
        }
    }

    /// <summary>
    /// Controla la existencia de un blog por su nombre en la db.
    /// Si no existe, se crea el blog y se guarda en la db
    /// </summary>
    /// <param name="blogCreate">Es un viewModel con los datos necesarios para crear un blog</param>
    /// <returns>
    ///     En caso de que el blog existe,devuelve un resultado fallido con un mensaje de error.
    ///     Caso contrario devuelve el blog creado en forma de viewmodel
    ///  </returns>

    public async Task<Result<BlogViewModel>> CreateAsync(BlogCreateViewModel blogCreate)
    {
        try
        {
            var authorizationResult = await _userAuthorizationService.AuthorizeUserAsync(blogCreate.OwnerBlogId);

            if (!authorizationResult.IsSucces) return Result<BlogViewModel>.Failure(authorizationResult.ErrorMessage, authorizationResult.State);

            bool nameBlogInUse = await _blogRepository.NameInUseAsync(blogCreate.Name);

            if (nameBlogInUse) return Result<BlogViewModel>.Failure(Message.blogname_in_use, State.NameInUse);

            var blog = _mapper.Map<Blog>(blogCreate);

            await _blogRepository.CreateAsync(blog);

            blog = await _blogRepository.GetBlogAsync(blog.Id);

            var blogViewModels = _mapper.Map<BlogViewModel>(blog);

            return Result<BlogViewModel>.Succes(blogViewModels);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error inesperado: " + ex.ToString());
            return Result<BlogViewModel>.Failure(Message.unexpected_error, State.InternalServerError);
        }
    }


    /*
        En este caso devuelvo el estado del fallo, para poder devolver 
        el código correspondiente en el controller
    */

    public async Task<Result> UpdateAsync(BlogUpdateViewModel blogUpdate)
    {
        try
        {
            var authorizationResult = await _userAuthorizationService.AuthorizeUserBlogAsync(blogUpdate.IdBlog);

            if (!authorizationResult.IsSucces) return Result.Failure(authorizationResult.ErrorMessage, authorizationResult.State);

            bool blogExists = await _blogRepository.ExistsAsync(blogUpdate.IdBlog);

            if (!blogExists) return Result.Failure(Message.blog_not_exist, State.NotExist);

            bool nameInUse = await _blogRepository.NameInUseAsync(blogUpdate.IdBlog, blogUpdate.Name);

            if (nameInUse) return Result.Failure(Message.blogname_in_use, State.NameInUse);

            var blog = _mapper.Map<Blog>(blogUpdate);

            await _blogRepository.UpdateAsync(blog);

            return Result.Succes(Message.blog_updated);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error inesperado: " + ex.ToString());
            return Result.Failure(Message.unexpected_error, State.InternalServerError);
        }

    }

    public async Task<Result> DeleteAsync(int blogId)
    {
        try
        {
            var authorizationResult = await _userAuthorizationService.AuthorizeUserBlogAsync(blogId);

            if (!authorizationResult.IsSucces) return Result.Failure(authorizationResult.ErrorMessage, authorizationResult.State);

            bool blogExists = await _blogRepository.ExistsAsync(blogId);

            if (!blogExists) return Result.Failure(Message.blog_not_exist, State.NotExist);

            await _blogRepository.DeleteAsync(blogId);

            return Result.Succes(Message.blog_deleted);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error inesperado: " + ex.ToString());
            return Result.Failure(Message.unexpected_error, State.InternalServerError);
        }

    }
}