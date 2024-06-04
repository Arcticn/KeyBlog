using KeyBlog.Data.Models.DTOs;
using KeyBlog.Data.Services;
using KeyBlog.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]

public class CategoryController : ControllerBase
{
    private readonly BlogPostService _blogPostService;
    private readonly CategoryService _categoryService;

    public CategoryController(BlogPostService blogPostService, CategoryService categoryService)
    {
        _blogPostService = blogPostService;
        _categoryService = categoryService;
    }

    [HttpGet("getCategories")]
    public async Task<IActionResult> GetCategories()
    {
        var categoryTree = await _categoryService.GetNode(false);
        return Ok(new
        {
            CategoryNodes = categoryTree
        });
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("getCategoriesAdmin")]
    public async Task<IActionResult> GetCategoriesAdmin()
    {
        var categoryTree = await _categoryService.GetNode(true);
        return Ok(new
        {
            CategoryNodes = categoryTree
        });
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("addCategory")]
    public async Task<IActionResult> AddCategory([FromBody] CategoryCreation tempCategory)
    {
        if (tempCategory == null)
        {
            return BadRequest("类别为空");
        }

        if (!await _categoryService.AddCategory(tempCategory))
        {
            return BadRequest("已经存在相同的类");
        };

        return Ok(new { message = "成功添加分类" });
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("editCategory")]
    public async Task<IActionResult> EditCategory(int id, string name)
    {
        await _categoryService.EditCategory(id, name);
        return Ok();
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("moveCategory")]
    public async Task<IActionResult> MoveCategory(int id, int parentId)
    {
        await _categoryService.MoveCategory(id, parentId);
        return Ok();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("deleteCategory")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        await _categoryService.DeleteCategory(id);
        return Ok();
    }
}