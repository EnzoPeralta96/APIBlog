using APIBlog.Data;
using APIBlog.Models;
using Microsoft.EntityFrameworkCore;

namespace APIBlog.Repository;
public class ReactionRepository : IReactionRepostiroy
{
    private readonly BlogDbContext _blogDbContext;
    public ReactionRepository(BlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }

    public async Task CreateAsync(Reaction reaction)
    {
        await _blogDbContext.Reactions.AddAsync(reaction);
        await _blogDbContext.SaveChangesAsync();
    }

    //public async Task UpdateAsync(int PostId){}
    //public async Task DeleteAsync(int PostId){}

    public async Task<Reaction> GetReactionAsync(int postId)
    {
        var reaction = await _blogDbContext.Reactions
                            .AsNoTracking()
                            .FirstOrDefaultAsync(r => r.PostId == postId);
        return reaction;
    }
  
    public async Task ILikeAsync(int id)
    {
        await _blogDbContext.Reactions
                .Where(r => r.PostId == id)
                .ExecuteUpdateAsync( 
                    s => s.SetProperty(r => r.NumberOfLikes, r => r.NumberOfLikes + 1));
    }

    public async Task IdontlikeAsync(int id)
    {
        await _blogDbContext.Reactions
                .Where(r => r.PostId == id)
                .ExecuteUpdateAsync( 
                    s => s.SetProperty(r => r.NumberOfLikes, r => r.NumberOfLikes - 1));
    }
    public async Task ViewsAsync(int id)
    {
        await _blogDbContext.Reactions
                .Where(r => r.PostId == id)
                .ExecuteUpdateAsync( 
                    s => s.SetProperty(r => r.NumberOfReading, r => r.NumberOfReading + 1));
    }

      /*public async Task ILikeAsync(int id)
    {
        var reaction = await _blogDbContext.Reactions.FindAsync(id);
        reaction.NumberOfLikes++;
        await _blogDbContext.SaveChangesAsync();
    }*/
}