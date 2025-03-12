namespace Common.Models;
public class Blog
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<Post> Posts { get; set; } = new List<Post>();
    public int UserId{get; set;}
    public User User{get; set;}
}