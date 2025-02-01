namespace Models;

public class Reaction
{
    public int Id{get; set;}
    public int NumberOfLikes {get; set;}
    public int NumberOfReading {get; set;}
    public int PostId{get; set;}
    public Post Post{get; set;}

}