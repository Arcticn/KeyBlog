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

  // [Navigate(nameof(ParentId))]
  // public Category? Parent { get; set; }

  // [Navigate(nameof(ParentId))]
  // public List<Category>? Children { get; set; } //树形查询使用

  /// <summary>
  /// 分类是否可见
  /// </summary>
  public bool Visible { get; set; } = true;

  ///<summary>
  ///Post集合导航
  ///</summary>
  [Navigate(nameof(Post.CategoryId))]
  public List<Post>? Posts { get; set; }

  public string? Categories { get; set; } = "0";//类的层级结构
}