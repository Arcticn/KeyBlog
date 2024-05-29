namespace KeyBlog.Data.Models.DTOs;

public class CategoryCreation
{
    public string? Name { get; set; }

    public int ParentId { get; set; }

    /// <summary>
    /// 分类是否可见
    /// </summary>
    public bool Visible { get; set; } = true;
}