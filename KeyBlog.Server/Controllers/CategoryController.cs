using KeyBlog.Data.Models.DTOs;
using KeyBlog.Data.Services;
using KeyBlog.Server.Services;
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
        var categoryTree = await _categoryService.GetNode();
        return Ok(new
        {
            CategoryNodes = categoryTree
        });
    }

    [HttpPost("addCategory")]
    public async Task<IActionResult> AddCategory([FromBody] CategoryCreation tempCategory)
    {
        if (tempCategory == null)
        {
            return BadRequest("Category is null");
        }

        if (!await _categoryService.AddCategory(tempCategory))
        {
            return BadRequest("Already exists the same category");
        };

        return Ok(new { message = "Catrgory added successfully" });
    }

    [HttpPut("editCategory")]
    public async Task<IActionResult> EditCategory(int id, string name)
    {
        await _categoryService.EditCategory(id, name);
        return Ok();
    }

    [HttpDelete("deleteCategory")]
    public async Task<IActionResult> DeleteCategory(int id){
        await _categoryService.DeleteCategory(id);
        return Ok();
    }
}