using KeyBlog.Data.Models.Entities;
using KeyBlog.Server.Services;
using KeyBlog.Server.Services.QueryFilters;
using Microsoft.AspNetCore.Mvc;

namespace KeyBlog.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BlogController : ControllerBase
{
    private readonly ArticleService _articleService;
    private readonly CategoryService _categoryService;

    public BlogController(ArticleService articleService, CategoryService categoryService)
    {
        _articleService = articleService;
        _categoryService = categoryService;
    }

    [HttpGet("articles")]
    public async Task<IActionResult> GetArticles([FromQuery] ArticleQueryParameters param, bool adminMode = false)
    {
        var pagedList = await _articleService.GetPagedList(param, adminMode);
        if (pagedList == null)
        {
            return NotFound(); // 返回 HTTP 404 状态码
        }
        return Ok(pagedList); // 返回 HTTP 200 状态码和数据
    }

    [HttpGet("articles/{id}")]
    public async Task<IActionResult> GetArticle(string id)
    {
        var article = await _articleService.GetArticleById(id);
        if (article == null)
        {
            return NotFound();
        }
        return Ok(article);
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
        var articles = await _articleService.GetPagedList(new ArticleQueryParameters
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
            Articles = articles
        });
    }

    [HttpPost("saveArticle")]
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