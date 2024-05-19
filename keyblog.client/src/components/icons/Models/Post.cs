namespace KeyBlog.Data.Models;

public class Post
{
    public string? Status { get; set; }
    public bool IsPublish { get; set; }
    public string? Path { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime LastUpdateTime { get; set; }
    public string? Categories { get; set; }
}