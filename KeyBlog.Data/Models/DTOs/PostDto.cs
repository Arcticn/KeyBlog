using System.ComponentModel.DataAnnotations;

namespace KeyBlog.Data.Models.DTOs;

public class PostDto
{
    public string? Id { get; set; }
    public string? Title { get; set; }

    public bool IsPublish { get; set; }

    public string? Summary { get; set; }

    [MaxLength(-1)]
    public string? Content { get; set; }

    public DateTime CreationTime { get; set; }

    public DateTime LastUpdateTime { get; set; }

    public int CategoryId { get; set; }

}