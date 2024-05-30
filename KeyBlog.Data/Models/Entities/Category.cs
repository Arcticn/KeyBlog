using FreeSql.DataAnnotations;
namespace KeyBlog.Data.Models.Entities;

public class Category
{
    [Column(IsIdentity = true, IsPrimary = true)]
    /*IsIdentity = true 表示该字段是自增字段。
      IsPrimary = true 表示该字段是主键。*/
    public int Id { get; set; }

    public string? Name { get; set; }

    public int ParentId { get; set; }

    [Navigate(nameof(ParentId))]
    public Category? Parent { get; set; }

    // [Navigate(nameof(ParentId))]
    // public List<Category>? Children { get; set; } //树形查询使用
    /// <summary>
    /// 分类是否可见
    /// </summary>
    public bool Visible { get; set; } = true;

    public List<Post>? Posts { get; set; } //集合导航
    public string? Categories { get; set; } = "0";//类的层级结构


}