namespace APIBlog.Models;

public class Reaction
{
    public Reaction()
    {
    }

    public Reaction(int postId)
    {
        PostId = postId;
        NumberOfLikes = 0;
        NumberOfReading = 0;
    }

    public int Id { get; set; }
    public int NumberOfLikes { get; set; }
    public int NumberOfReading { get; set; }
    public int PostId { get; set; }
    public Post Post { get; set; }
}