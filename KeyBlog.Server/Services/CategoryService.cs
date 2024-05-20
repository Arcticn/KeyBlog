﻿using FreeSql;
using KeyBlog.Data.Models.DTOs;
using KeyBlog.Data.Models.Entities;

namespace KeyBlog.Server.Services;

public class CategoryService
{
    private readonly IBaseRepository<Category> _cRepo;

    public CategoryService(IBaseRepository<Category> cRepo)
    {
        _cRepo = cRepo;
    }


    //_categoryRepo.Where(a => a.Id == categoryId)
    public Task<Category> GetCategory(int categoryId)
    {
        return _cRepo.Where(a => a.Id == categoryId).FirstAsync();
    }

    public async Task<List<CategoryNode>?> GetNodes()
    {
        var categoryList = await _cRepo.Select
            .IncludeMany(a => a.Articles.Select(p => new Article { Id = p.Id }))
            .ToListAsync();
        return GetNodes(categoryList, 0);
    }

    /// <summary>
    /// 生成文章分类树
    /// </summary>
    public List<CategoryNode>? GetNodes(List<Category> categoryList, int parentId = 0)
    {
        var categories = categoryList
            .Where(a => a.ParentId == parentId && a.Visible)
            .ToList();

        if (categories.Count == 0) return null;

        return categories.Select(category => new CategoryNode
        {
            Id = category.Id,
            Label = category.Name,
            Children = GetNodes(categoryList, category.Id)
        }).ToList();
    }

}