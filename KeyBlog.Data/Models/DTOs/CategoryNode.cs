namespace KeyBlog.Data.Models.DTOs;

public class CategoryNode
{
    public int Id { get; set; }
    public string? Label { get; set; } // 用于 el-menu 的显示名称

    public int ParentId { get; set; }
    public List<CategoryNode>? Children { get; set; } // 子节点列表
}
