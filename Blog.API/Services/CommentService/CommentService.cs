using System.Runtime.CompilerServices;
using Common.Models;
using APIBlog.Policies.Authorization;
using APIBlog.Repository;
using APIBlog.Shared;
using Common.ViewModels;
using AutoMapper;

namespace APIBlog.Services;
public class CommentService : ICommentService
{
    private readonly ILogger _logger;
    private readonly IUserAuthorizationService _userAuthorizationService;
    private readonly ICommentRepository _commentRepository;
    private readonly IPostRepository _postRepository;
    private readonly IMapper _mapper;
    public CommentService(IUserAuthorizationService userAuthorizationService, ICommentRepository commentRepository, IMapper mapper, IPostRepository postRepository, ILogger<CommentService> logger)
    {
        _userAuthorizationService = userAuthorizationService;
        _commentRepository = commentRepository;
        _mapper = mapper;
        _postRepository = postRepository;
        _logger = logger;
    }

    public async Task<Result<List<CommentViewModel>>> GetCommentsByPost(int postId)
    {
        try
        {
            var postExists = await _postRepository.ExistsAsync(postId);

            if (!postExists) return Result<List<CommentViewModel>>.Failure(Message.post_not_exist, State.NotExist);

            var comments = await _commentRepository.GetCommentsByPost(postId);

            var commentsViewModel = _mapper.Map<List<CommentViewModel>>(comments);

            return Result<List<CommentViewModel>>.Succes(commentsViewModel);
        }
        catch (Exception ex)
        {
             _logger.LogError("Error inesperado: " + ex.ToString());
            return Result<List<CommentViewModel>>.Failure(Message.unexpected_error, State.InternalServerError);
        }
    }

    public async Task<Result<CommentViewModel>> GetComment(int commentId)
    {
        try
        {
            var comment = await _commentRepository.GetCommentAsync(commentId);

            if (comment is null) return Result<CommentViewModel>.Failure(Message.commet_not_exist, State.NotExist);

            var commentViewModel = _mapper.Map<CommentViewModel>(comment);

            return Result<CommentViewModel>.Succes(commentViewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error inesperado: " + ex.ToString());
            return Result<CommentViewModel>.Failure(Message.unexpected_error, State.InternalServerError);
        }
    }


    public async Task<Result<CommentViewModel>> CreateAsync(CommentCreateViewModel commentCreate)
    {
        try
        {
            var authorizationResult = await _userAuthorizationService.AuthorizeUserAsync(commentCreate.UserId);

            if (!authorizationResult.IsSucces) return Result<CommentViewModel>.Failure(authorizationResult.ErrorMessage, authorizationResult.State);

            var postExists = await _postRepository.ExistsAsync(commentCreate.PostId);

            if (!postExists) return Result<CommentViewModel>.Failure(Message.post_not_exist, State.NotExist);

            var comment = _mapper.Map<Comment>(commentCreate);

            await _commentRepository.CreateAsync(comment);

            comment = await _commentRepository.GetCommentAsync(comment.Id);

            var commentViewModel = _mapper.Map<CommentViewModel>(comment);

            return Result<CommentViewModel>.Succes(commentViewModel);
        }
        catch (Exception ex)
        {
             _logger.LogError("Error inesperado: " + ex.ToString());
            return Result<CommentViewModel>.Failure(Message.unexpected_error, State.InternalServerError);
        }
    }

    public async Task<Result> UpdateAsync(CommentUpdateViewModel commentUpdate)
    {
        try
        {
            var authorizationResult = await _userAuthorizationService.AuthorizeUserCommentUpdateAsync(commentUpdate.Id);

            if (!authorizationResult.IsSucces) return Result.Failure(authorizationResult.ErrorMessage, authorizationResult.State);

            var commentExits = await _commentRepository.ExistsAsync(commentUpdate.Id);

            if (!commentExits) return Result.Failure(Message.commet_not_exist, State.NotExist);

            var comment = _mapper.Map<Comment>(commentUpdate);

            await _commentRepository.UpdateAsync(comment);

            return Result.Succes(Message.comment_updated);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error inesperado: " + ex.ToString());
            return Result.Failure(Message.unexpected_error, State.InternalServerError);
        }

    }

    public async Task<Result> DeleteAsync( int commentId)
    {
        try
        {
            var authorizationResult = await _userAuthorizationService.AuthorizeUserCommentDeleteAsync(commentId);

            if (!authorizationResult.IsSucces) return Result.Failure(authorizationResult.ErrorMessage, authorizationResult.State);

            var commentExits = await _commentRepository.ExistsAsync(commentId);

            if (!commentExits) return Result.Failure(Message.commet_not_exist, State.NotExist);

            await _commentRepository.DeleteAsync(commentId);

            return Result.Succes(Message.comment_deleted);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error inesperado: " + ex.ToString());
            return Result.Failure(Message.unexpected_error, State.InternalServerError);
        }
    }
}