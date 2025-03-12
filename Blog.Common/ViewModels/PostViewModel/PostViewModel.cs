namespace Common.ViewModels;
public class PostViewModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string DateCreate { get; set; }
    public int Likes { get; set; }
    public int Views { get; set; }
    public int BlogId { get; set; }
    public UserViewModel User{get; set;}
}