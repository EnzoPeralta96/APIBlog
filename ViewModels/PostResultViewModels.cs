using APIBlog.Models;

namespace APIBlog.ViewModels;

public class PostResultViewModels
{
    public BlogState BlogState { get; set; }
    public List<Post> Posts { get; set; }

    public PostResultViewModels(BlogState blogState, List<Post>? posts = null)
    {
        BlogState = blogState;
        Posts = posts;
    }


}