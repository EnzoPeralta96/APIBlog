namespace Common.ViewModels;
public class CommentViewModel
{
    public int Id { get; set; }
    public string Comentary { get; set; }
    public UserViewModel User { get; set; }
}