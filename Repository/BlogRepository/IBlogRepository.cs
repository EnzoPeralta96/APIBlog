using APIBlog.Models;
namespace APIBlog.Repository;
public interface IBlogRepository
{
    Task CreateAsync(Blog blog);
    Task UpdateAsync(int id, Blog blog);
    Task DeleteAsync(int id);
    Task<Blog> GetBlogAsync(int id);
    Task<Blog> GetBlogAsync(string name);
    Task<List<Blog>> BlogsAsync();
    Task<List<Post>> PostsByBlogAsync(int id);
    Task<List<Post>> PostsByBlogAsync(string name);
    Task<bool> NameInUseAsync(string name);
    Task<bool> ExistsAsync(int id);
}