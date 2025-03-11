using APIBlog.Shared;
using APIBlog.ViewModels;

namespace APIBlog.Services;
public interface ICommentService
{
    Task<Result<List<CommentViewModel>>> GetCommentsByPost(int postId);
    Task<Result<CommentViewModel>> GetComment(int commentId);
    Task<Result<CommentViewModel>> CreateAsync(CommentCreateViewModel commentCreate);
    Task<Result> UpdateAsync(CommentUpdateViewModel commentUpdate);
    Task<Result> DeleteAsync(int commentId);
}