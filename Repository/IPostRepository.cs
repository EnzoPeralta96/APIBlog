using APIBlog.Models;

namespace APIBlog.Repository;
public interface IPostRepository
{
    Task<Post> GetPostAsync(int id);
    Task CreateAsync(Post post);
    Task UpdateAsync(int id, Post post);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);

    //Task<Post> GetLastPostAsync();
    //Task LikeAsync(int id);
    //Task ReadAsync(int id);
}