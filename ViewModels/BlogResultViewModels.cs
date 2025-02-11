using APIBlog.Models;

namespace APIBlog.ViewModels;
public class BlogResultViewModels
{
    public BlogState BlogState { get; set; }
    public Blog Blog { get; set; }
    public BlogResultViewModels(BlogState blogState, Blog? blog = null)
    {
        BlogState = blogState;
        Blog = blog;
    }
}