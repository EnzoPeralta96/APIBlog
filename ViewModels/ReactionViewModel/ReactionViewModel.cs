using System.ComponentModel;

namespace APIBlog.ViewModels;

public class ReactionViewModel
{
    [DisplayName("Likes")]
    public int NumberOfLikes { get; set; }
    [DisplayName("Views")]
    public int NumberOfReading { get; set; }
}