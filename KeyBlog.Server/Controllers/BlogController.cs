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

    [HttpGet("posts")]
    public async Task<IActionResult> GetPosts([FromQuery] PostQueryParameters param, bool adminMode = false)
    {
        var pagedList = await _blogPostService.GetPagedList(param, adminMode);
        if (pagedList == null)
        {
            return NotFound(); // 返回 HTTP 404 状态码
        }
        return Ok(pagedList); // 返回 HTTP 200 状态码和数据
    }

    [HttpGet("posts/{id}")]
    public async Task<IActionResult> GetPost(string id)
    {
        var post = await _blogPostService.GetPostById(id);
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

        var categoryNodes = await _categoryService.GetTreeList();
        var posts = await _blogPostService.GetPagedList(new PostQueryParameters
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
        var categoryTree = await _categoryService.GetTreeList();
        return Ok(new
        {
            CategoryNodes = categoryTree
        });
    }

    [HttpPost("addCategory")]
    public async Task<ActionResult> AddCategory([FromBody] CategoryCreation tempCategory)
    {
        if (tempCategory == null)
        {
            return BadRequest("Category is null");
        }

        if (!await _categoryService.addCategory(tempCategory))
        {
            return BadRequest("Already exists the same category");
        };

        return Ok(new { message = "Catrgory added successfully" });
    }


    [HttpPost("savePost")]
    public async Task<IActionResult> SavePost([FromBody] PostCreation newPost)
    {
        if (newPost == null || string.IsNullOrEmpty(newPost.Content))
        {
            return BadRequest("Content is null or empty");
        }

        // 将内容保存到数据库或文件
        // 示例：保存到文件
        // System.IO.File.WriteAllText("D:/123.txt", newPost.Content);


        Post tempPost = new Post
        {
            Id = GuidUtils.GetGuid(),
            Title = newPost.Title,
            Summary = newPost.Summary,
            IsPublish = newPost.IsPublish,
            Content = newPost.Content,
            CreationTime = newPost.CreationTime,
            CategoryId = newPost.CategoryId
        };

        await _blogPostService.InsetPost(tempPost);

        return Ok();
    }
}
