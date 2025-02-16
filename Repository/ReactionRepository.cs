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
   /* public async Task CreateAsync(int postId)
    {
        Reaction reaction = new Reaction(postId);
        await _blogDbContext.Reactions.AddAsync(reaction);
        await _blogDbContext.SaveChangesAsync();
    }*/

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
        var reaction = await _blogDbContext.Reactions.FindAsync(id);
        reaction.NumberOfLikes++;
        await _blogDbContext.SaveChangesAsync();
    }

    public async Task IdontlikeAsync(int id)
    {
        var reaction = await _blogDbContext.Reactions.FindAsync(id);
        reaction.NumberOfLikes--;
        await _blogDbContext.SaveChangesAsync();
    }
    public async Task ViewsAsync(int id)
    {
        var reaction = await _blogDbContext.Reactions.FindAsync(id);
        reaction.NumberOfReading++;
        await _blogDbContext.SaveChangesAsync();
    }
}