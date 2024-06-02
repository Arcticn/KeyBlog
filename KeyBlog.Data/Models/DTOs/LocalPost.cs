using FreeSql.DataAnnotations;
using KeyBlog.Data.Extensions;
using System.ComponentModel.DataAnnotations;

namespace KeyBlog.Data.Models.DTOs;

/// <summary>
/// 博客文章
/// </summary>
public class LocalPost
{
    public string? Id { get; set; }

    /// <summary>
    /// 标题
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// 文章标记，提取原markdown文件的文件名前缀，用于区分文章状态，《（已发表）.*》
    /// </summary>
    public string? Status { get; set; }

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
            (Content != null ? "    "+Markdig.Markdown.ToPlainText(Content).Limit(200) : "");
        }
        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                _summary = value.Limit(200);
            }
            else if (Content != null)
            {
                _summary = "    "+Markdig.Markdown.ToPlainText(Content).Limit(200);
            }
            else
            {
                _summary = "";
            }
        }

    }

    /// <summary>
    /// 博客在导入前的相对路径
    /// <para>如："系列/AspNetCore开发笔记"</para>
    /// </summary>
    public string? Path { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    private static DateTime _creatime;
    public DateTime CreationTime
    {
        get => _creatime;
        set
        {
            if (_creatime == DateTime.MinValue)
            {
                _creatime = value;
                LastUpdateTime = _creatime;
            }
        }
    }

    /// <summary>
    /// 上次更新时间
    /// </summary>
    public DateTime LastUpdateTime { get; set; }

    /// <summary>
    /// Category导航属性
    /// </summary>

    public int CategoryId { get; set; }

}