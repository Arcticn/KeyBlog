namespace KeyBlog.Data.Models.DTOs;

public class CategoryDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int ParentId { get; set; }
    // public List<PostDto>? Posts { get; set; }
    public List<CategoryDto>? Children { get; set; } // 子节点列表
}
