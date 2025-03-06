namespace APIBlog.ViewModels;

public class BlogViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public UserViewModel OwnerBlog {get; set;}
}