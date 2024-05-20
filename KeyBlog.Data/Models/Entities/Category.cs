using FreeSql.DataAnnotations;
namespace KeyBlog.Data.Models.Entities;

public class Category
{
    [Column(IsIdentity = true, IsPrimary = true)]
    public int Id { get; set; }

    public string Name { get; set; }

    public int ParentId { get; set; }
    public Category? Parent { get; set; }

    /// <summary>
    /// 分类是否可见
    /// </summary>
    public bool Visible { get; set; } = true;

    public List<Article> Posts { get; set; }
}