using KeyBlog.Data.Models.DTOs;
using KeyBlog.Data.Models.Entities;
using KeyBlog.Data.Services;
using KeyBlog.Data.Utils;
using KeyBlog.Server.Services;
using KeyBlog.Server.Services.QueryFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]

public class PostController : ControllerBase
{
    private readonly BlogPostService _blogPostService;
    private readonly CategoryService _categoryService;

    public PostController(BlogPostService blogPostService, CategoryService categoryService)
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
    public async Task<IActionResult> GetPost([FromRoute] string id)
    {
        var post = await _blogPostService.GetPost(id);
        if (post == null)
        {
            return NotFound();
        }
        return Ok(post);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("savePost")]
    public async Task<IActionResult> SavePost([FromBody] PostDto newPost)
    {
        if (newPost == null || string.IsNullOrEmpty(newPost.Content))
        {
            return BadRequest("内容为空");
        }

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

        if (newPost.Id == null)
        {
            await _blogPostService.InsetPost(tempPost);
            return Ok(new { id = tempPost.Id });
        }
        else
        {
            tempPost.Id = newPost.Id;
            tempPost.LastUpdateTime = DateTime.Now;
            await _blogPostService.EditPost(tempPost);
            return Ok(new { id = tempPost.Id });
        }

        // return Ok();
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("updatePost")]
    public async Task<IActionResult> UpdatePost([FromBody] PostDto post)
    {
        if (post == null || string.IsNullOrEmpty(post.Content))
        {
            return BadRequest("内容为空");
        }

        Post tempPost = new Post
        {
            Id = post.Id,
            Title = post.Title,
            Summary = post.Summary,
            IsPublish = post.IsPublish,
            Content = post.Content,
            CreationTime = post.CreationTime,
            LastUpdateTime = post.LastUpdateTime,
            CategoryId = post.CategoryId
        };

        await _blogPostService.EditPost(tempPost);

        return Ok();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("deletePost")]
    public async Task<IActionResult> DeletePost(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest("id为空");
        }

        await _blogPostService.DeletePost(id);

        return Ok();
    }
}