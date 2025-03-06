using APIBlog.Models;
using APIBlog.Policies.Authorization;
using APIBlog.Repository;
using APIBlog.Shared;
using APIBlog.ViewModels;
using AutoMapper;

namespace APIBlog.Services;
public class PostService : IPostService
{
    private readonly IUserAuthorizationService _userAuthorizationService;
    private readonly IBlogRepository _blogRepository;
    private readonly IPostRepository _postRepository;
    private readonly IMapper _mapper;
    public PostService(IUserAuthorizationService userAuthorizationService, IPostRepository postRepository, IBlogRepository blogRepository, IMapper mapper)
    {
        _userAuthorizationService = userAuthorizationService;
        _blogRepository = blogRepository;
        _postRepository = postRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<PostViewModel>>> GetPostsByBlogAsync(int blogId)
    {
        try
        {
            bool blogExists = await _blogRepository.ExistsAsync(blogId);

            if (!blogExists) return Result<List<PostViewModel>>.Failure($"El blog id = {blogId} no existe", State.NotExist);

            var posts = await _postRepository.GetPostsByBlogAsync(blogId);

            var postsViewModel = _mapper.Map<List<PostViewModel>>(posts);

            return Result<List<PostViewModel>>.Succes(postsViewModel);
        }
        catch (Exception ex)
        {
            return Result<List<PostViewModel>>.Failure($"Error: {ex.Message}", State.InternalServerError);
        }
    }

    public async Task<Result<PostViewModel>> GetPostAsync(int id)
    {
        try
        {
            bool postExists = await _postRepository.ExistsAsync(id);

            if (!postExists) return Result<PostViewModel>.Failure($"El post con id = {id} no existe", State.NotExist);

            var post = await _postRepository.GetPostAsync(id);

            var postViewModel = _mapper.Map<PostViewModel>(post);

            return Result<PostViewModel>.Succes(postViewModel);
        }
        catch (Exception ex)
        {
            return Result<PostViewModel>.Failure($"Error: {ex.Message}", State.InternalServerError);
        }
    }

    public async Task<Result<PostViewModel>> CreateAsync(PostCreateViewModel postCreate)
    {
        try
        {
            var authorizationResult = await _userAuthorizationService.AuthorizeUserAsync(postCreate.OwnerPostId);

            if (!authorizationResult.IsSucces) return Result<PostViewModel>.Failure(authorizationResult.ErrorMessage, authorizationResult.State);

            bool blogExists = await _blogRepository.ExistsAsync(postCreate.BlogId);

            if (!blogExists) return Result<PostViewModel>.Failure($"El blog id = {postCreate.BlogId} no existe", State.NotExist);

            var post = _mapper.Map<Post>(postCreate);

            await _postRepository.CreateAsync(post);

            var postViewModel = _mapper.Map<PostViewModel>(post);

            return Result<PostViewModel>.Succes(postViewModel);
        }
        catch (Exception ex)
        {
            return Result<PostViewModel>.Failure($"Error: {ex.Message}", State.InternalServerError);
        }
    }

    public async Task<Result> UpdateAsync(PostUpdateViewModel postUpdate)
    {
        try
        {
            var authorizationUpdateResult = await _userAuthorizationService.AuthorizeUserPostRequestAsync(postUpdate.OwnerPostId, postUpdate.PostId);

            if (!authorizationUpdateResult.IsSucces) return Result.Failure(authorizationUpdateResult.ErrorMessage, authorizationUpdateResult.State);


            bool postExists = await _postRepository.ExistsAsync(postUpdate.PostId);

            if (!postExists) return Result.Failure($"El post con id = {postUpdate.PostId} no existe", State.NotExist);

            var post = _mapper.Map<Post>(postUpdate);

            await _postRepository.UpdateAsync(post);

            return Result.Succes();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Error: {ex.Message}", State.InternalServerError);
        }
    }

    public async Task<Result> DeleteAsync(int ownerId, int postId)
    {
        try
        {
            var authorizationDeleteResult = await _userAuthorizationService.AuthorizeUserPostRequestAsync(ownerId, postId);

            if (!authorizationDeleteResult.IsSucces) return Result.Failure(authorizationDeleteResult.ErrorMessage, authorizationDeleteResult.State);

            bool postExists = await _postRepository.ExistsAsync(postId);

            if (!postExists) return Result.Failure($"El post con id = {postId} no existe", State.NotExist);

            await _postRepository.DeleteAsync(postId);

            return Result.Succes("Post eliminado con éxito");
        }
        catch (Exception ex)
        {
            return Result.Failure($"Error: {ex.Message}", State.InternalServerError);
        }
    }

}
