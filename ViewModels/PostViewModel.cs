

namespace APIBlog.ViewModels;
public class PostViewModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string DateCreate { get; set; }
    public ReactionViewModel Reaction { get; set; }
}