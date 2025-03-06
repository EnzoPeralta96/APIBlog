using APIBlog.Models;
namespace APIBlog.Repository;
public interface IBlogRepository
{
    Task<List<Blog>> BlogsAsync(int userId);
    Task<Blog> GetBlogAsync(int id);
    Task CreateAsync(Blog blog);
    Task UpdateAsync(Blog blog);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<bool> NameInUseAsync(string name);
    Task<bool> NameInUseAsync(int idBlog, string name);
    Task<bool> IsOwnerBlog(int idUser, int idBlog);
}