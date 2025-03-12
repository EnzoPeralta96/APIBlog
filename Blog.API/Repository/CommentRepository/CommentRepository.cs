using APIBlog.Data;
using Common.Models;
using Microsoft.EntityFrameworkCore;

namespace APIBlog.Repository;

public class CommentRepository : ICommentRepository
{
    private readonly BlogDbContext _blogDbContext;
    public CommentRepository(BlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }

    public async Task<List<Comment>> GetCommentsByPost(int postId)
    {
        return await _blogDbContext.Comments
                    .AsNoTracking()
                    .Where(c => c.PostId == postId)
                    .Include(c => c.User)
                    .ToListAsync();
    }
    public async Task<Comment> GetCommentAsync(int commentId)
    {
        return await _blogDbContext.Comments
                         .AsNoTracking()
                         .Include(c => c.User)
                         .FirstOrDefaultAsync(c => c.Id == commentId);
    }
    public async Task CreateAsync(Comment comment)
    {
        await _blogDbContext.Comments.AddAsync(comment);
        await _blogDbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Comment comment)
    {
        await _blogDbContext.Comments
                .Where(c => c.Id == comment.Id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(c => c.Comentary, comment.Comentary)
                );
    }

    public async Task DeleteAsync(int commentId)
    {
        await _blogDbContext.Comments
                .Where(c => c.Id == commentId)
                .ExecuteDeleteAsync();
    }

    public Task<bool> ExistsAsync(int commentId)
    {
        return _blogDbContext.Comments
                .AsNoTracking()
                .AnyAsync(c => c.Id == commentId);
    }

    public bool IsOwnerComment(int userId, int commentId)
    {
        return _blogDbContext.Comments
                .AsNoTracking()
                .Any(c => c.UserId == userId && c.Id == commentId);
    }

    public bool IsOwnerPostByComment(int userId, int commentId)
    {
        return _blogDbContext.Comments
                .AsNoTracking()
                .Any(c => c.Post.UserId == userId && c.Id == commentId);
    }
}