﻿namespace KeyBlog.Server.Services.QueryFilters;

/// <summary>
/// 博客文章请求参数
/// </summary>
public class PostQueryParameters : QueryParameters
{
    /// <summary>
    /// 是否已发布（仅管理员可以查询和管理未发布文章）
    /// </summary>
    public bool? IsPublish { get; set; } = null;

    /// <summary>
    /// 分类ID
    /// </summary>
    public int CategoryId { get; set; } = 0;

    /// <summary>
    /// 排序字段
    /// </summary>
    public new string? SortBy { get; set; } = "-LastUpdateTime";
}