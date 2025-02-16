using APIBlog.Models;

namespace APIBlog.Repository;
public interface IPostRepository
{
    public Task CreateAsync(Post post);
    public Task UpdateAsync(int id, Post post);
    public Task DeleteAsync(int id);
    public Task<Post> GetPostAsync(int id);
    public Task<Post> GetPostWhitReactionAsync(int id);
    public Task<Post> GetLastPostAsync();
    public Task ReactPostAsync(int id);
    public Task ReadPostAsync(int id);
}