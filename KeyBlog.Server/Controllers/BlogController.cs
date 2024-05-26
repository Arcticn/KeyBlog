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
    private readonly PostService _postService;
    private readonly CategoryService _categoryService;

    public BlogController(PostService postService, CategoryService categoryService)
    {
        _postService = postService;
        _categoryService = categoryService;
    }

    [HttpGet("posts")]
    public async Task<IActionResult> GetPosts([FromQuery] PostQueryParameters param, bool adminMode = false)
    {
        var pagedList = await _postService.GetPagedList(param, adminMode);
        if (pagedList == null)
        {
            return NotFound(); // 返回 HTTP 404 状态码
        }
        return Ok(pagedList); // 返回 HTTP 200 状态码和数据
    }

    [HttpGet("posts/{id}")]
    public async Task<IActionResult> GetPost(string id)
    {
        var post = await _postService.GetPostById(id);
        if (post == null)
        {
            return NotFound();
        }
        return Ok(post);
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
        var posts = await _postService.GetPagedList(new PostQueryParameters
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
            Posts = posts
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


    [HttpPost("savePost")]
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