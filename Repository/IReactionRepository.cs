using APIBlog.Models;

namespace APIBlog.Repository;
public interface IReactionRepostiroy
{
   // public Task CreateAsync(int postId);
    //public Task UpdateAsync(int PostId);
    //public Task DeleteAsync(int PostId);
    public Task<Reaction> GetReactionAsync(int postId);
    public Task ILikeAsync(int id);
    public Task IdontlikeAsync(int id);
    public Task ViewsAsync(int id);
}