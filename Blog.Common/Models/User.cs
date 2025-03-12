namespace Common.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public int RoleId { get; set; }
    public Role Role { get; set; }
    public List<Blog> Blogs { get; set; } = new List<Blog>();
    public List<Post> Posts {get; set;} = new List<Post>();
    public List<Comment> Comments {get; set;} = new List<Comment>();
}