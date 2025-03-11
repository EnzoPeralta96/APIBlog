using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using APIBlog.Data;
using APIBlog.Models;
using APIBlog.Repository;
using Microsoft.AspNetCore.Routing.Tree;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

public class PostRepository : IPostRepository
{
    private readonly BlogDbContext _blogDbContext;

    public PostRepository(BlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }

    public async Task<List<Post>> GetPostsByBlogAsync(int blogId)
    {
        return await _blogDbContext.Posts
                    .AsNoTracking()
                    .Where(pst => pst.BlogId == blogId)
                    .Include(pst => pst.User)
                    .ToListAsync();
    }
    public async Task<Post> GetPostAsync(int id)
    {
        return await _blogDbContext.Posts
                        .AsNoTracking()
                        .Include(pst => pst.User)
                        .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task CreateAsync(Post post)
    {
        await _blogDbContext.Posts.AddAsync(post);
        await _blogDbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Post post)
    {
        await _blogDbContext.Posts
                .Where(pst => pst.Id == post.Id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(pst => pst.Title, post.Title)
                    .SetProperty(pst => pst.Content, post.Content)
                );
    }

    public async Task DeleteAsync(int id)
    {
        await _blogDbContext.Posts
                .Where(pst => pst.Id == id)
                .ExecuteDeleteAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _blogDbContext.Posts
                    .AsNoTracking()
                    .AnyAsync(p => p.Id == id);
    }

    public bool IsOwnerPost(int idUser, int idPost)
    {
        return _blogDbContext.Posts
                         .AsNoTracking()
                         .Any(post => post.UserId == idUser && post.Id == idPost);
    }

    public bool IsOwnerBlogByPost(int userId, int postId)
    {
        return _blogDbContext.Posts
                .AsNoTracking()
                .Any(p => p.Blog.UserId == userId && p.Id == postId);
    }


}