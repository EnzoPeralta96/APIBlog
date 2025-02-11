using System.Threading.Tasks;
using APIBlog.Data;
using APIBlog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace APIBlog.Repository;

public class BlogRepository : IBlogRepository
{
    private readonly BlogDbContext _blogDbContext;

    public BlogRepository(BlogDbContext blogDb)
    {
        _blogDbContext = blogDb;
    }

    public async Task CreateAsync(Blog blog)
    {
        await _blogDbContext.Blogs.AddAsync(blog);
        await _blogDbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(int id, Blog blog)
    {
        var searched_blog = await _blogDbContext.Blogs.FindAsync(id);
        searched_blog.Name = blog.Name;
        searched_blog.Description = blog.Description;
        await _blogDbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var searched_blog = await _blogDbContext.Blogs.FindAsync(id);
        _blogDbContext.Blogs.Remove(searched_blog);
        await _blogDbContext.SaveChangesAsync();
    }
    public async Task<Blog> GetBlogAsync(int id)
    {
        var blog = await _blogDbContext.Blogs
                        .AsNoTracking()
                        .FirstOrDefaultAsync(bl => bl.Id == id);
        return blog;
    }

    public async Task<Blog> GetBlogAsync(string name)
    {
        var blog = await _blogDbContext.Blogs
                        .AsNoTracking()
                        .FirstOrDefaultAsync(bl => bl.Name == name);
        return blog;
    }

    public async Task<List<Blog>> BlogsAsync()
    {
        var blogs = await _blogDbContext.Blogs
                    .AsNoTracking()
                    .ToListAsync();
        return blogs;
    }
    public async Task<List<Post>> PostsByBlogAsync(int id)
    {
        var posts = await _blogDbContext.Posts
                        .Where(p => p.BlogId == id)
                        .AsNoTracking()
                        .ToListAsync();
        return posts;
    }

    public async Task<List<Post>> PostsByBlogAsync(string name)
    {
        var posts = await _blogDbContext.Posts
                    .Where(p => p.Blog.Name == name)
                    .AsNoTracking()
                    .ToListAsync();
        return posts;
    }

}


