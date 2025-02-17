using System.Reflection.PortableExecutable;
using APIBlog.Data;
using APIBlog.Models;
using APIBlog.Repository;
using Microsoft.EntityFrameworkCore;

public class PostRepository:IPostRepository
{
    private readonly BlogDbContext _blogDbContext;

    public PostRepository(BlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }

    public async Task CreateAsync(Post post)
    {
        await _blogDbContext.Posts.AddAsync(post);
        await _blogDbContext.SaveChangesAsync();
    }
    
    public async Task UpdateAsync(int id, Post post)
    {
        var post_update = await _blogDbContext.Posts.FindAsync(id);
        post_update.Title = post.Title;
        post_update.Content = post.Content;
        await _blogDbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var post = await _blogDbContext.Posts.FindAsync(id);
        _blogDbContext.Posts.Remove(post);
        await _blogDbContext.SaveChangesAsync();
    }
    public async Task<Post> GetPostAsync(int id)
    {
        var post = await _blogDbContext.Posts
                        .AsNoTracking()
                        .Include(r => r.Reaction)
                        .FirstOrDefaultAsync(p => p.Id == id);
        return post;
    }

    /*public async Task<Post> GetLastPostAsync()
    {
        var post = await _blogDbContext.Posts
                        .AsNoTracking()
                        .OrderByDescending(pt => pt.Id)
                        .FirstOrDefaultAsync();
        return post;
    }*/

    public async Task LikeAsync(int id)
    {
        var post = await _blogDbContext.Posts.FindAsync(id);
        post.Reaction.NumberOfLikes++;
        await _blogDbContext.SaveChangesAsync();
    }

    public async Task ReadAsync(int id)
    {
        var post = await _blogDbContext.Posts.FindAsync(id);
        post.Reaction.NumberOfReading++;
        await _blogDbContext.SaveChangesAsync();
    }
    
}