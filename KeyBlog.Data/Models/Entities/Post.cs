using FreeSql.DataAnnotations;
using KeyBlog.Data.Extensions;
using System.ComponentModel.DataAnnotations;

namespace KeyBlog.Data.Models.Entities;

/// <summary>
/// 博客文章
/// </summary>
public class Post
{
    [Column(IsIdentity = false, IsPrimary = true)]
    public string? Id { get; set; }

    /// <summary>
    /// 标题
    /// </summary>
    public string? Title { get; set; }

    // todo 新增 tag 功能

    /// <summary>
    /// 是否发表（不发表的话就是草稿状态）
    /// </summary>
    public bool IsPublish { get; set; }

    /// <summary>
    /// 内容（markdown格式）
    /// </summary>
    [MaxLength(-1)]
    public string? Content { get; set; }


    /// <summary>
    /// 梗概
    /// </summary>
    private string? _summary;
    public string? Summary
    {
        get
        {
            return _summary ??
            (Content != null ? Markdig.Markdown.ToPlainText(Content).Limit(200) : "");
        }
        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                _summary = value.Limit(200);
            }
            else if (Content != null)
            {
                _summary = Markdig.Markdown.ToPlainText(Content).Limit(200);
            }
            else
            {
                _summary = "";
            }
        }

    }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreationTime { get; set; }

    /// <summary>
    /// 上次更新时间
    /// </summary>
    public DateTime LastUpdateTime { get; set; }

    /// <summary>
    /// Category导航属性
    /// </summary>

    public int CategoryId { get; set; }
    [Navigate(nameof(CategoryId))]
    public Category? Category { get; set; }

}