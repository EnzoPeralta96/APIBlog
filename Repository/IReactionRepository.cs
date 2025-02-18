using APIBlog.Models;

namespace APIBlog.Repository;
public interface IReactionRepostiroy
{
    Task CreateAsync(Reaction reaction);
    //Task UpdateAsync(int PostId);
    //Task DeleteAsync(int PostId);
    Task<Reaction> GetReactionAsync(int postId);
    Task ILikeAsync(int postId);
    Task IdontlikeAsync(int postId);
    Task ViewsAsync(int postId);
}