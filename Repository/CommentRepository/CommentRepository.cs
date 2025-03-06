using APIBlog.Data;

namespace APIBlog.Repository;

public class CommentRepository : ICommentRepository
{
    private readonly BlogDbContext _blogDbContext;
    public CommentRepository(BlogDbContext blogDbContext)
    {
        _blogDbContext = blogDbContext;
    }
}