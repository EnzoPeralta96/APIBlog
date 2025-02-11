using APIBlog.Models;

namespace APIBlog.Repository;

public interface IBlogRepository
{
    public Task CreateAsync(Blog blog);
    public Task UpdateAsync(int id, Blog blog);
    public Task DeleteAsync(int id);
    public Task<Blog> GetBlogAsync(int id);
    public Task<Blog> GetBlogAsync(string name);
    public Task<List<Blog>> BlogsAsync();
    public Task<List<Post>> PostsByBlogAsync(int id);
    public Task<List<Post>> PostsByBlogAsync(string name);

}