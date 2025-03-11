using APIBlog.Models;

namespace APIBlog.Repository;
public interface IPostRepository
{
    Task<List<Post>> GetPostsByBlogAsync(int blogId);
    Task<Post> GetPostAsync(int id);
    Task CreateAsync(Post post);
    Task UpdateAsync(Post post);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    bool IsOwnerPost(int userId, int postId);

    bool IsOwnerBlogByPost(int userId, int postId);

}