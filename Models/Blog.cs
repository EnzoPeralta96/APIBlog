using APIBlog.ViewModels;
namespace APIBlog.Models;

public class Blog
{
    public Blog()
    {
    }

    public Blog(BlogViewModels blog)
    {
        Name = blog.Name;
        Description = blog.Description;
    }

    public int Id{get; set;}
    public string Name{get; set;}
    public string Description {get; set;}
    public List<Post> Posts {get; set;} = new List<Post>();
}