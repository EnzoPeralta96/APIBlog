namespace APIBlog.ViewModels;

public class BlogViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int OwnerId {get; set;}
    //public List<PostViewModel> Posts { get; set; } = new List<PostViewModel>();

}