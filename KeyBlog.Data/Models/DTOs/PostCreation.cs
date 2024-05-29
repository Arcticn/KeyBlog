using System.ComponentModel.DataAnnotations;

namespace KeyBlog.Data.Models.DTOs;

public class PostCreation
{
    public string? Title { get; set; }

    public bool IsPublish { get; set; }

    public string? Summary { get; set; }

    [MaxLength(-1)]
    public string? Content { get; set; }

     public DateTime CreationTime { get; set; }
    /// <summary>
    /// 分类ID
    /// </summary>
    public int CategoryId { get; set; }

}