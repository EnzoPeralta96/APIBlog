using APIBlog.Data;
using APIBlog.Models;
using Microsoft.EntityFrameworkCore;
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

    public async Task UpdateAsync(Blog blog)
    {
        await _blogDbContext.Blogs
                .Where(bl => bl.Id == blog.Id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(bl => bl.Name, blog.Name)
                    .SetProperty(bl => bl.Description, blog.Description)
                );
    }


    public async Task<Blog> GetBlogAsync(int id)
    {
        var blog = await _blogDbContext.Blogs
                        .AsNoTracking()
                        .Include(bl => bl.Posts)
                        .FirstOrDefaultAsync(bl => bl.Id == id);
        return blog;
    }

    public async Task DeleteAsync(int id)
    {
        await _blogDbContext.Blogs
             .Where(bl => bl.Id == id)
             .ExecuteDeleteAsync();
    }


    public async Task<Blog> GetBlogAsync(string name)
    {
        var blog = await _blogDbContext.Blogs
                        .AsNoTracking()
                        .Include(bl => bl.Posts)
                        .FirstOrDefaultAsync(bl => bl.Name == name);
        return blog;
    }

    public async Task<List<Blog>> BlogsAsync(int userId)
    {
        return await _blogDbContext.Blogs
                    .AsNoTracking()
                    .Where(bl => bl.UserId == userId)
                    .ToListAsync();
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

    public async Task<bool> NameInUseAsync(int idBlog, string name)
    {
        return await _blogDbContext.Blogs
                    .AsNoTracking()
                    .Where(bl => bl.Id != idBlog)
                    .AnyAsync(bl => bl.Name == name);
    }

    public async Task<bool> NameInUseAsync(string name)
    {
        return await _blogDbContext.Blogs
                    .AsNoTracking()
                    .AnyAsync(bl => bl.Name == name);

    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _blogDbContext.Blogs
                        .AsNoTracking()
                        .AnyAsync(bl => bl.Id == id);
    }

   

    /*public async Task UpdateAsync(int id, Blog blog)
    {
        var searched_blog = await _blogDbContext.Blogs.FindAsync(id);
        searched_blog.Name = blog.Name;
        searched_blog.Description = blog.Description;
        await _blogDbContext.SaveChangesAsync();
    }*/

    /* public async Task DeleteAsync(int id)
     {
         var searched_blog = await _blogDbContext.Blogs.FindAsync(id);
         _blogDbContext.Blogs.Remove(searched_blog);
         await _blogDbContext.SaveChangesAsync();
     }*/

    /*public async Task DeleteAsync(int id)
{
   var blogToDelete = new Blog { Id = id };
   _blogDbContext.Blogs.Attach(blogToDelete);
   _blogDbContext.Blogs.Remove(blogToDelete);
   await _blogDbContext.SaveChangesAsync();
}
  public async Task UpdateAsync(int id, Blog blog)
    {
        var postUpdate = new Blog { Id = id, Name = blog.Name, Description = blog.Description };
        _blogDbContext.Blogs.Attach(postUpdate);
        _blogDbContext.Entry(postUpdate).Property(bl => bl.Name).IsModified = true;
        _blogDbContext.Entry(postUpdate).Property(bl => bl.Description).IsModified = true;
        await _blogDbContext.SaveChangesAsync();
    }
*/

}


