
namespace APIBlog.Models;
public class Comment
{
    public int Id{get; set;}
    public string Comentary{get; set;}
    public int UserId{get; set;}
    public User User{get; set;}
    public int PostId{get; set;}
    public Post Post{get; set;}
}
