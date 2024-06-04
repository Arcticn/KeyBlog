using KeyBlog.Data.Models.DTOs;
using KeyBlog.Data.Models.Entities;
using KeyBlog.Data.Services;
using KeyBlog.Server.Services;
using KeyBlog.Data.Utils;
using KeyBlog.Server.Services.QueryFilters;
using Microsoft.AspNetCore.Mvc;

namespace KeyBlog.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BlogController : ControllerBase
{
    private readonly BlogPostService _blogPostService;
    private readonly CategoryService _categoryService;

    public BlogController(BlogPostService blogPostService, CategoryService categoryService)
    {
        _blogPostService = blogPostService;
        _categoryService = categoryService;
    }


    [HttpGet("lists")]
    public async Task<IActionResult> List(int categoryId = 0, int page = 1, int pageSize = 6,
             string sortType = "asc", string sortBy = "CreationTime", string search = "", bool adminMode = false)
    {
        var currentCategory = categoryId == 0
            ? new Category { Id = 0, Name = "All" }
            : await _categoryService.GetCategory(categoryId);

        if (currentCategory == null)
        {
            return NotFound(new { message = $"Category {categoryId} does not exist!" });
        }

        if (!currentCategory.Visible)
        {
            return BadRequest(new { message = $"Category {categoryId} is not available!" });
        }

        var categoryNodes = await _categoryService.GetNode();
        var posts = await _blogPostService.GetPagedList(new PostQueryParameters
        {
            CategoryId = categoryId,
            Page = page,
            PageSize = pageSize,
            Search = search,
            SortBy = sortType == "desc" ? $"-{sortBy}" : sortBy
        }, adminMode);

        return Ok(new
        {
            CurrentCategory = currentCategory,
            CurrentCategoryId = categoryId,
            CategoryNodes = categoryNodes,
            SortType = sortType,
            SortBy = sortBy,
            Posts = posts
        });
    }

}
