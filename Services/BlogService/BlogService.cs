using APIBlog.Models;
using APIBlog.Policies.Authorization;
using APIBlog.Repository;
using APIBlog.Shared;
using APIBlog.ViewModels;
using AutoMapper;

namespace APIBlog.Services;
public class BlogService : IBlogService
{
    private readonly IUserAuthorizationService _userAuthorizationService;
    private readonly IBlogRepository _blogRepository;
    private readonly IMapper _mapper;
    public BlogService(IUserAuthorizationService userAuthorizationService, IBlogRepository blogRepository, IUserRepository userRepository, IMapper mapper)
    {

        _blogRepository = blogRepository;
        _userAuthorizationService = userAuthorizationService;
        _mapper = mapper;
    }

    public async Task<Result<BlogViewModel>> BlogAsync(int id)
    {
        try
        {
            var blog = await _blogRepository.GetBlogAsync(id);

            if (blog is null) return Result<BlogViewModel>.Failure($"El blog con id = {id} no existe", State.NotExist);

            var blogViewModel = _mapper.Map<BlogViewModel>(blog);

            return Result<BlogViewModel>.Succes(blogViewModel);
        }
        catch (Exception ex)
        {
            return Result<BlogViewModel>.Failure($"Error: {ex.Message}", State.InternalServerError);
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

            if (nameBlogInUse) return Result<BlogViewModel>.Failure("El blog ya existe", State.NameInUse);

            var blog = _mapper.Map<Blog>(blogCreate);

            await _blogRepository.CreateAsync(blog);

            /*
            Una vez que se crea el post, EF hace un seguimiento de blog
            por lo que luego de crearlo, ya tiene su id
            */
            var blogViewModels = _mapper.Map<BlogViewModel>(blog);

            return Result<BlogViewModel>.Succes(blogViewModels);
        }
        catch (Exception ex)
        {
            return Result<BlogViewModel>.Failure($"Error inesperado: {ex.Message}", State.InternalServerError);
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
            var authorizationResult = await _userAuthorizationService.AuthorizeUserAsync(blogUpdate.OwnerBlogId, blogUpdate.IdBlog);

            if (!authorizationResult.IsSucces) return Result.Failure(authorizationResult.ErrorMessage, authorizationResult.State);

            bool blogExists = await _blogRepository.ExistsAsync(blogUpdate.IdBlog);

            if (!blogExists) return Result.Failure("El blog no existe", State.NotExist);

            bool nameInUse = await _blogRepository.NameInUseAsync(blogUpdate.IdBlog, blogUpdate.Name);

            if (nameInUse) return Result.Failure("Nombre de blog en uso", State.NameInUse);

            var blog = _mapper.Map<Blog>(blogUpdate);

            await _blogRepository.UpdateAsync(blog);

            return Result.Succes();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Error: {ex.Message}", State.InternalServerError);
        }

    }

    public async Task<Result> DeleteAsync(int ownerId, int blogId)
    {
        try
        {
            var authorizationResult = await _userAuthorizationService.AuthorizeUserAsync(ownerId, blogId);

            if (!authorizationResult.IsSucces) return Result.Failure(authorizationResult.ErrorMessage, authorizationResult.State);

            bool blogExists = await _blogRepository.ExistsAsync(blogId);

            if (!blogExists) return Result.Failure($"El Blog con id = {blogId} no existe", State.NotExist);

            await _blogRepository.DeleteAsync(blogId);

            return Result.Succes("Blog eliminado con éxito");
        }
        catch (Exception ex)
        {
            return Result.Failure($"Error: {ex.Message}", State.InternalServerError);
        }

    }

    public async Task<Result<List<BlogViewModel>>> BlogsAsync(int userId)
    {
        try
        {
            var blogs = await _blogRepository.BlogsAsync(userId);

            var blogsViewModel = _mapper.Map<List<BlogViewModel>>(blogs);

            return Result<List<BlogViewModel>>.Succes(blogsViewModel);
        }
        catch (Exception ex)
        {
            return Result<List<BlogViewModel>>.Failure($"Error: {ex.Message}", State.InternalServerError);
        }
    }

    public async Task<Result<List<PostViewModel>>> PostsByBlogAsync(int id)
    {
        try
        {
            bool blogExists = await _blogRepository.ExistsAsync(id);

            if (!blogExists) return Result<List<PostViewModel>>.Failure($"El Blog con id = {id} no existe", State.NotExist);

            List<Post> posts = await _blogRepository.PostsByBlogAsync(id);

            var postView = _mapper.Map<List<PostViewModel>>(posts);

            return Result<List<PostViewModel>>.Succes(postView);
        }
        catch (Exception ex)
        {
            return Result<List<PostViewModel>>.Failure($"Error: {ex.Message}", State.InternalServerError);
        }

    }
}