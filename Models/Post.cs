namespace APIBlog.Models;

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTimeOffset DateCreate { get; set; }
    public int Likes{get;set;}
    public int Views {get; set;}
    public List<Comment> Comments {get; set;} = new List<Comment>();
    public int BlogId { get; set; }
    public Blog Blog { get; set; }
    public int UserId {get; set;}
    public User User {get; set;}
}