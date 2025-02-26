using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using APIBlog.Data;
using APIBlog.Models;
using APIBlog.Repository;
using Microsoft.AspNetCore.Routing.Tree;
using Microsoft.EntityFrameworkCore;

public class PostRepository : IPostRepository
{
    private readonly BlogDbContext _blogDbContext;

    public PostRepository(BlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }

    public async Task<Post> GetPostAsync(int id)
    {
        var post = await _blogDbContext.Posts
                        .AsNoTracking()
                        .Include(r => r.Reaction)
                        .FirstOrDefaultAsync(p => p.Id == id);
        return post;
    }

    public async Task CreateAsync(Post post)
    {
        await _blogDbContext.Posts.AddAsync(post);
        await _blogDbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(int id, Post post)
    {
        var postToUpdate = new Post { Id = id, Title = post.Title, Content = post.Content };
        _blogDbContext.Posts.Attach(postToUpdate);
        _blogDbContext.Entry(postToUpdate).Property(p => p.Title).IsModified = true;
        _blogDbContext.Entry(postToUpdate).Property(p => p.Content).IsModified = true;
        await _blogDbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var postToDelete = new Post { Id = id };
        _blogDbContext.Posts.Attach(postToDelete);
        _blogDbContext.Posts.Remove(postToDelete);
        await _blogDbContext.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _blogDbContext.Posts
                    .AsNoTracking()
                    .AnyAsync(p => p.Id == id);
    }

    /* public async Task UpdateAsync(int id, Post post)
     {
         var post_update = await _blogDbContext.Posts.FindAsync(id);
         post_update.Title = post.Title;
         post_update.Content = post.Content;
         await _blogDbContext.SaveChangesAsync();
     }*/


    /* public async Task DeleteAsync(int id)
     {
         var post = await _blogDbContext.Posts.FindAsync(id);
         _blogDbContext.Posts.Remove(post);
         await _blogDbContext.SaveChangesAsync();
     }*/


    /*public async Task<Post> GetLastPostAsync()
    {
        var post = await _blogDbContext.Posts
                        .AsNoTracking()
                        .OrderByDescending(pt => pt.Id)
                        .FirstOrDefaultAsync();
        return post;
    }*/

    /* public async Task LikeAsync(int id)
     {
         var post = await _blogDbContext.Posts.FindAsync(id);
         post.Reaction.NumberOfLikes++;
         await _blogDbContext.SaveChangesAsync();
     }

     public async Task LikeAsync(int id)
     {
         await _blogDbContext.Po
     }

     public async Task ReadAsync(int id)
     {
         var post = await _blogDbContext.Posts.FindAsync(id);
         post.Reaction.NumberOfReading++;
         await _blogDbContext.SaveChangesAsync();
     }*/

}