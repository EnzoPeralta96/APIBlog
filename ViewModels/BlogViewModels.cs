
using System.ComponentModel.DataAnnotations;
using APIBlog.Models;
namespace APIBlog.ViewModels;
public class BlogViewModels
{
    public BlogViewModels()
    {
    }

    public BlogViewModels(Blog blog)
    {
        Name = blog.Name;
        Description = blog.Description;
    }
    
    [Required][MaxLength(50)]
    public string Name{get; set;}

    [Required][MaxLength(150)]
    public string Description {get; set;}
    
}