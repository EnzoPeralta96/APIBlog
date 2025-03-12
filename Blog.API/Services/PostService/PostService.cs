using APIBlog.Policies.Authorization;
using APIBlog.Repository;
using APIBlog.Shared;
using AutoMapper;
using Common.Models;
using Common.ViewModels;

namespace APIBlog.Services;
public class PostService : IPostService
{
    private readonly ILogger _logger;
    private readonly IUserAuthorizationService _userAuthorizationService;
    private readonly IBlogRepository _blogRepository;
    private readonly IPostRepository _postRepository;
    private readonly IMapper _mapper;
    public PostService(IUserAuthorizationService userAuthorizationService, IPostRepository postRepository, IBlogRepository blogRepository, IMapper mapper, ILogger<PostService> logger)
    {
        _userAuthorizationService = userAuthorizationService;
        _blogRepository = blogRepository;
        _postRepository = postRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Result<List<PostViewModel>>> GetPostsByBlogAsync(int blogId)
    {
        try
        {
            bool blogExists = await _blogRepository.ExistsAsync(blogId);

            if (!blogExists) return Result<List<PostViewModel>>.Failure(Message.blog_not_exist, State.NotExist);

            var posts = await _postRepository.GetPostsByBlogAsync(blogId);

            var postsViewModel = _mapper.Map<List<PostViewModel>>(posts);

            return Result<List<PostViewModel>>.Succes(postsViewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error inesperado: " + ex.ToString());
            return Result<List<PostViewModel>>.Failure(Message.unexpected_error, State.InternalServerError);
        }
    }

    public async Task<Result<PostViewModel>> GetPostAsync(int id)
    {
        try
        {
            bool postExists = await _postRepository.ExistsAsync(id);

            if (!postExists) return Result<PostViewModel>.Failure(Message.post_not_exist, State.NotExist);

            var post = await _postRepository.GetPostAsync(id);

            var postViewModel = _mapper.Map<PostViewModel>(post);

            return Result<PostViewModel>.Succes(postViewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error inesperado: " + ex.ToString());
            return Result<PostViewModel>.Failure(Message.unexpected_error, State.InternalServerError);
        }
    }

    public async Task<Result<PostViewModel>> CreateAsync(PostCreateViewModel postCreate)
    {
        try
        {
            var authorizationResult = await _userAuthorizationService.AuthorizeUserAsync(postCreate.OwnerPostId);

            if (!authorizationResult.IsSucces) return Result<PostViewModel>.Failure(authorizationResult.ErrorMessage, authorizationResult.State);

            bool blogExists = await _blogRepository.ExistsAsync(postCreate.BlogId);

            if (!blogExists) return Result<PostViewModel>.Failure(Message.blog_not_exist, State.NotExist);

            var post = _mapper.Map<Post>(postCreate);

            await _postRepository.CreateAsync(post);

            post = await _postRepository.GetPostAsync(post.Id);

            var postViewModel = _mapper.Map<PostViewModel>(post);

            return Result<PostViewModel>.Succes(postViewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error inesperado: " + ex.ToString());
      
            return Result<PostViewModel>.Failure(Message.unexpected_error, State.InternalServerError);
        }
    }

    public async Task<Result> UpdateAsync(PostUpdateViewModel postUpdate)
    {
        try
        {
            var authorizationUpdateResult = await _userAuthorizationService.AuthorizeUserPostUpdateAsync(postUpdate.PostId);

            if (!authorizationUpdateResult.IsSucces) return Result.Failure(authorizationUpdateResult.ErrorMessage, authorizationUpdateResult.State);

            bool postExists = await _postRepository.ExistsAsync(postUpdate.PostId);

            if (!postExists) return Result.Failure(Message.post_not_exist, State.NotExist);

            var post = _mapper.Map<Post>(postUpdate);

            await _postRepository.UpdateAsync(post);

            return Result.Succes(Message.post_updated);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error inesperado: " + ex.ToString());
             return Result.Failure(Message.unexpected_error, State.InternalServerError);
        }
    }

    public async Task<Result> DeleteAsync(int postId)
    {
        try
        {
            var authorizationDeleteResult = await _userAuthorizationService.AuthorizeUserPostDeleteAsync(postId);

            if (!authorizationDeleteResult.IsSucces) return Result.Failure(authorizationDeleteResult.ErrorMessage, authorizationDeleteResult.State);

            bool postExists = await _postRepository.ExistsAsync(postId);

            if (!postExists) return Result.Failure(Message.post_not_exist, State.NotExist);

            await _postRepository.DeleteAsync(postId);

            return Result.Succes(Message.post_deleted);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error inesperado: " + ex.ToString());
            return Result.Failure(Message.unexpected_error, State.InternalServerError);
        }

        
    }

}
