using APIBlog.Models;
using APIBlog.Repository;
using APIBlog.Shared;
using APIBlog.ViewModels;
using AutoMapper;

namespace APIBlog.Services;
public class PostService : IPostService
{
    private readonly IPostRepository _postRepository;
    private readonly IBlogRepository _blogRepository;
    private readonly IReactionRepostiroy _reactionRepository;
    private readonly IMapper _mapper;
    public PostService(IPostRepository postRepository, IBlogRepository blogRepository, IReactionRepostiroy reactionRepository, IMapper mapper)
    {
        _postRepository = postRepository;
        _blogRepository = blogRepository;
        _reactionRepository = reactionRepository;
        _mapper = mapper;
    }

    public async Task<Result<PostViewModel>> GetPostAsync(int id)
    {
        try
        {
            bool postExists = await _postRepository.ExistsAsync(id);

            if (!postExists) return Result<PostViewModel>.Failure($"El post con id = {id} no existe", State.NotExist);

            await _reactionRepository.ViewsAsync(id);

            var post = await _postRepository.GetPostAsync(id);

            var postViewModel = _mapper.Map<PostViewModel>(post);

            return Result<PostViewModel>.Succes(postViewModel);
        }
        catch (Exception ex)
        {
            return Result<PostViewModel>.Failure($"Error: {ex.Message}", State.InternalServerError);
        }
    }

    public async Task<Result<PostViewModel>> CreateAsync(int idBlog, PostRequestViewModel postRequest)
    {
        try
        {
            bool blogExists = await _blogRepository.ExistsAsync(idBlog);

            if (!blogExists) return Result<PostViewModel>.Failure($"El blog id = {idBlog} no existe", State.NotExist);

            var post = _mapper.Map<Post>(postRequest, options => options.Items["BlogId"] = idBlog);

            await _postRepository.CreateAsync(post);

            var reaction = new Reaction(post.Id);
            await _reactionRepository.CreateAsync(reaction);

            post.Reaction = reaction;

            var postViewModel = _mapper.Map<PostViewModel>(post);

            return Result<PostViewModel>.Succes(postViewModel);
        }
        catch (Exception ex)
        {
            return Result<PostViewModel>.Failure($"Error: {ex.Message}", State.InternalServerError);
        }
    }

    public async Task<Result> UpdateAsync(int id, PostRequestViewModel postRequest)
    {
        try
        {
            bool postExists = await _postRepository.ExistsAsync(id);
            if (!postExists) return Result.Failure($"El post con id = {id} no existe", State.NotExist);
            
            var post = _mapper.Map<Post>(postRequest);
            await _postRepository.UpdateAsync(id, post);
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
            bool postExists = await _postRepository.ExistsAsync(id);
            if (!postExists) return Result.Failure($"El post con id = {id} no existe", State.NotExist);
            await _postRepository.DeleteAsync(id);
            return Result.Succes("Post eliminado con éxito");
        }
        catch (Exception ex)
        {
            return Result.Failure($"Error: {ex.Message}", State.InternalServerError);
        }
    }

    public async Task<Result> LikeAsync(int postId)
    {
        try
        {
            bool postExists = await _postRepository.ExistsAsync(postId);
            if (!postExists) return Result.Failure($"El post con id = {postId} no existe", State.NotExist);
            await _reactionRepository.ILikeAsync(postId);
            return Result.Succes();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Error: {ex.Message}", State.InternalServerError);
        }
    }
}

/*
    public Task CreateAsync(Post post);
    public Task UpdateAsync(int id, Post post);
    public Task DeleteAsync(int id);
    public Task<Post> GetPostAsync(int id);
    public Task ReactPostAsync(int id);
    public Task ReadPostAsync(int id);
*/