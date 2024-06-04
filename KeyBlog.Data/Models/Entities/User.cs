
using FreeSql.DataAnnotations;
namespace KeyBlog.Data.Models.Entities;
public class User
{
    [Column(IsIdentity = true, IsPrimary = true)]
    public int Id { get; set; }
    public bool IsAdmin { get; set; } = false;
    public string? Username { get; set; }

    /// <summary>
    /// 存储密码的哈希值
    /// </summary>
    public string? PasswordHash { get; set; }
}