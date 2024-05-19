using KeyBlog.Data.Models;
using KeyBlog.Server.Services;
using KeyBlog.Server.Services.QueryFilters;
using Microsoft.AspNetCore.Mvc;

namespace KeyBlog.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BlogController : ControllerBase
{
    private readonly PostService _postService;

    public BlogController(PostService postService)
    {
        _postService = postService;
    }

    [HttpGet("oldposts")]
    public ActionResult<IEnumerable<Post>> GetPosts()
    {
        var posts = _postService.GetAllPosts();
        if (posts == null || !posts.Any())
        {
            return NotFound(); // 返回 HTTP 404 状态码
        }
        return Ok(posts); // 返回 HTTP 200 状态码和数据
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
}
