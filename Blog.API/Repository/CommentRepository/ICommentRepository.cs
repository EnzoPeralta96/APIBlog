using Common.Models;

namespace APIBlog.Repository;
public interface ICommentRepository
{
    Task<List<Comment>> GetCommentsByPost(int postId);
    Task<Comment> GetCommentAsync(int commentId);
    Task CreateAsync(Comment comment);
    Task UpdateAsync(Comment comment);
    Task DeleteAsync(int commentId);
    Task<bool> ExistsAsync(int commentId);
    bool IsOwnerComment(int userId, int commentId);

    bool IsOwnerPostByComment(int userId, int commentId);

}