using APIBlog.Models;
using APIBlog.Repository;
using APIBlog.Shared;
using APIBlog.ViewModels;
using AutoMapper;

namespace APIBlog.Services;
public class BlogService : IBlogService
{
    private readonly IBlogRepository _blogRepository;
    private readonly IMapper _mapper;
    public BlogService(IBlogRepository blogRepository, IMapper mapper)
    {
        _blogRepository = blogRepository;
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
    /// <param name="blogRequest">Es un viewModel con los datos necesarios para crear un blog</param>
    /// <returns>
    ///     En caso de que el blog existe,devuelve un resultado fallido con un mensaje de error.
    ///     Caso contrario devuelve el blog creado en forma de viewmodel
    ///  </returns>

    public async Task<Result<BlogViewModel>> CreateAsync(BlogRequestViewModel blogRequest)
    {
        try
        {
            bool nameBlogInUse = await _blogRepository.NameBlogInUse(blogRequest.Name);

            if (nameBlogInUse) return Result<BlogViewModel>.Failure("El blog ya existe", State.NameInUse);

            var blog = _mapper.Map<Blog>(blogRequest);

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
        //Controlo si no hay un blog con el mismo nombre

    }


    /*
        En este caso devuelvo el estado del fallo, para poder devolver 
        el código correspondiente en el controller
    */

    public async Task<Result> UpdateAsync(int id, BlogRequestViewModel blogRequest)
    {
        try
        {
            bool blogExists = await _blogRepository.BlogExists(id);

            if (!blogExists) return Result.Failure("El blog no existe", State.NotExist);

            var existingBlog = await _blogRepository.GetBlogAsync(blogRequest.Name);

            if (existingBlog is not null && existingBlog.Id != id) return Result.Failure("Nombre de blog en uso", State.NameInUse);

            var blog = _mapper.Map<Blog>(blogRequest);

            await _blogRepository.UpdateAsync(id, blog);

            return Result.Succes();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Error: {ex.Message}", State.InternalServerError);
        }

    }

    public async Task<Result> DeleteAsync(int id)
    {
        try
        {
            bool blogExists = await _blogRepository.BlogExists(id);

            if (!blogExists) return Result.Failure($"El Blog con id = {id} no existe",State.NotExist);

            await _blogRepository.DeleteAsync(id);

            return Result.Succes("Blog eliminado con éxito");
        }
        catch (Exception ex)
        {
            return Result.Failure($"Error: {ex.Message}", State.InternalServerError);
        }

    }

    public async Task<Result<List<BlogViewModel>>> BlogsAsync()
    {
        try
        {
            var blogs = await _blogRepository.BlogsAsync();

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
            bool blogExists = await _blogRepository.BlogExists(id);

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