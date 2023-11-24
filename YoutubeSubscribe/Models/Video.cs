namespace YoutubeSubscribe.Models;

public class Video
{
    public string? Title { get; set; }
    
    public string? Description { get; set; }

    public int Length { get; set; }

    public int NumOfLikes { get; set; }

    public Channel? Channel { get; set; }
    
}