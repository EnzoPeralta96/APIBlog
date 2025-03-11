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

    public async Task<List<Blog>> BlogsAsync(int userId)
    {
        return await _blogDbContext.Blogs
                    .AsNoTracking()
                    .Include(bl => bl.User)
                    .Where(bl => bl.UserId == userId)
                    .ToListAsync();
    }

    public async Task<Blog> GetBlogAsync(int id)
    {
        return await _blogDbContext.Blogs
                        .AsNoTracking()
                        .Include(bl => bl.User)
                        .FirstOrDefaultAsync(bl => bl.Id == id);
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
    public async Task DeleteAsync(int id)
    {
        await _blogDbContext.Blogs
             .Where(bl => bl.Id == id)
             .ExecuteDeleteAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _blogDbContext.Blogs
                        .AsNoTracking()
                        .AnyAsync(bl => bl.Id == id);
    }

    //Se usa para actualizacion
    public async Task<bool> NameInUseAsync(int idBlog, string name)
    {
        return await _blogDbContext.Blogs
                    .AsNoTracking()
                    .AnyAsync(bl => bl.Id != idBlog && bl.Name == name);
    }

    //Se usa para creacion
    public async Task<bool> NameInUseAsync(string name)
    {
        return await _blogDbContext.Blogs
                    .AsNoTracking()
                    .AnyAsync(bl => bl.Name == name);
    }
    public bool IsOwnerBlog(int idUser, int idBlog)
    {
        return  _blogDbContext.Blogs
                        .AsNoTracking()
                        .Any(bl => bl.Id == idBlog && bl.UserId == idUser);
    }

}


