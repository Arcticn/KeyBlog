using KeyBlog.Data.Extensions;

namespace KeyBlog.Data.Models.DTOs;
public class UserDto
{
    public string? Username { get; set; }

    /// <summary>
    /// 存储密码的哈希值
    /// </summary>
    private string? _passwordHash;
    public string? PasswordHash
    {
        get => _passwordHash;
        set
        {
            _passwordHash = value?.ToSM3Hash();
        }
    }
}