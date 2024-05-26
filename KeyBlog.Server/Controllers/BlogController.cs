using KeyBlog.Data.Models.DTOs;
using KeyBlog.Data.Models.Entities;
using KeyBlog.Server.Services;
using KeyBlog.Server.Services.QueryFilters;
using Microsoft.AspNetCore.Mvc;

namespace KeyBlog.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BlogController : ControllerBase
{
    private readonly PostService _blogService;
    private readonly CategoryService _categoryService;

    public BlogController(PostService blogService, CategoryService categoryService)
    {
        _blogService = blogService;
        _categoryService = categoryService;
    }

    [HttpGet("blogs")]
    public async Task<IActionResult> GetBlogs([FromQuery] BlogQueryParameters param, bool adminMode = false)
    {
        var pagedList = await _blogService.GetPagedList(param, adminMode);
        if (pagedList == null)
        {
            return NotFound(); // 返回 HTTP 404 状态码
        }
        return Ok(pagedList); // 返回 HTTP 200 状态码和数据
    }

    [HttpGet("blogs/{id}")]
    public async Task<IActionResult> GetBlog(string id)
    {
        var blog = await _blogService.GetBlogById(id);
        if (blog == null)
        {
            return NotFound();
        }
        return Ok(blog);
    }

    [HttpGet("lists")]
    public async Task<IActionResult> List(int categoryId = 0, int page = 1, int pageSize = 6,
             string sortType = "asc", string sortBy = "CreationTime")
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

        var categoryNodes = await _categoryService.GetNodes();
        var blogs = await _blogService.GetPagedList(new BlogQueryParameters
        {
            CategoryId = categoryId,
            Page = page,
            PageSize = pageSize,
            SortBy = sortType == "desc" ? $"-{sortBy}" : sortBy
        });

        return Ok(new
        {
            CurrentCategory = currentCategory,
            CurrentCategoryId = categoryId,
            CategoryNodes = categoryNodes,
            SortType = sortType,
            SortBy = sortBy,
            Blogs = blogs
        });
    }

    [HttpGet("getCategories")]
    public async Task<IActionResult> GetCategories()
    {
        var categoryNodes = await _categoryService.GetNodes();
        return Ok(new
        {
            CategoryNodes = categoryNodes
        });
    }

    [HttpPost("addCategory")]
    public IActionResult AddCategory([FromBody] TempCategory tempCategory)
    {
        if (tempCategory == null)
        {
            return BadRequest("Category is null");
        }

        if (!_categoryService.addCategory(tempCategory))
        {
            return BadRequest("Already exists the same category");
        };

        return Ok(new { message = "Catrgory added successfully" });
    }


    [HttpPost("saveBlog")]
    public IActionResult SaveContent([FromBody] ContentModel model)
    {
        if (model == null || string.IsNullOrEmpty(model.Content))
        {
            return BadRequest("Content is null or empty");
        }

        // 将内容保存到数据库或文件
        // 示例：保存到文件
        System.IO.File.WriteAllText("D:/123.txt", model.Content);

        return Ok(new { message = "Content saved successfully" });
    }
}
public class ContentModel
{
    public string Content { get; set; }
}