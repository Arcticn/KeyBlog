﻿using FreeSql;
using KeyBlog.Data.Models.DTOs;
using KeyBlog.Data.Models.Entities;

namespace KeyBlog.Data.Services;

public class CategoryService
{

    private readonly IBaseRepository<Category> _cRepo;

    public CategoryService(IBaseRepository<Category> cRepo)
    {
        _cRepo = cRepo;
    }

    //_categoryRepo.Where(a => a.Id == categoryId)
    public async Task<Category> GetCategory(int categoryId)
    {
        var category = await _cRepo.Select.Where(a => a.Id == categoryId).FirstAsync();
        return category;
    }

    public async Task<List<CategoryNode>?> GetTreeList()
    {
        // 树形查询功能冗余，暂时弃用
        // var categoryList = await _cRepo.Select
        // .IncludeMany(a => a.Posts.Select(p => new Post { Id = p.Id }))
        // .ToTreeListAsync();
        var categoryList = await _cRepo.Select
            .IncludeMany(a => a.Posts.Select(p => new Post { Id = p.Id }))
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
            ParentId = category.ParentId,
            Children = GetNodes(categoryList, category.Id)
        }).ToList();
    }

    public async Task<bool> addCategory(CategoryCreation tempCategory)
    {
        var existingCategory = await _cRepo.Where(a => a.Name == tempCategory.Name && a.ParentId == tempCategory.ParentId)
                                .FirstAsync();
        if (existingCategory == null)
        {
            await _cRepo.InsertAsync(new Category
            {
                Name = tempCategory.Name,
                ParentId = tempCategory.ParentId
            });
            var temp = await _cRepo.Where(a => a.Name == tempCategory.Name && a.ParentId == tempCategory.ParentId)
                                .FirstAsync();
            temp.Categories = GetCategoryHierarchy(temp.Id);
            await _cRepo.UpdateAsync(temp);
            return true;
        }

        else return false;//已经存在相同分类

    }

    public string GetCategoryHierarchy(int categoryId)
    {
        var category = _cRepo.Where(c => c.Id == categoryId).First();
        if (category == null) return string.Empty;

        var hierarchy = new Stack<int>();
        while (category != null)
        {
            hierarchy.Push(category.Id);
            category = _cRepo.Where(c => c.Id == category.ParentId).First();
        }

        return string.Join(",", hierarchy);
    }
}