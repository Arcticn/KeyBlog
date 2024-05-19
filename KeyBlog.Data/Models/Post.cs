using FreeSql.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace KeyBlog.Data.Models;

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

    /// <summary>
    /// 文章链接，设置后可以通过以下形式访问文章
    /// <para> http://.../p/post-slug1 </para>
    /// </summary>
    [MaxLength(150)]
    public string? Slug { get; set; }

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
    /// 梗概
    /// </summary>
    public string? Summary { get; set; }

    /// <summary>
    /// 内容（markdown格式）
    /// </summary>
    [MaxLength(-1)]
    public string? Content { get; set; }

    /// <summary>
    /// 博客在导入前的相对路径
    /// <para>如："系列/AspNetCore开发笔记"</para>
    /// </summary>
    public string? Path { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreationTime { get; set; }

    /// <summary>
    /// 上次更新时间
    /// </summary>
    public DateTime LastUpdateTime { get; set; }

    /// <summary>
    /// 分类ID
    /// </summary>
    public int CategoryId { get; set; }

    /// <summary>
    /// 分类
    /// </summary>
    public Category? Category { get; set; }

    public string? Categories { get; set; }

}