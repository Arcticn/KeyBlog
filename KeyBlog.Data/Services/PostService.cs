using FreeSql;
using KeyBlog.Data.Models.Entities;

namespace KeyBlog.Data.Services;

public class PostService
{
    protected readonly IBaseRepository<Post> _postRepo;
    protected readonly IBaseRepository<Category> _categoryRepo;

    public PostService(IBaseRepository<Post> postRepo, IBaseRepository<Category> categoryRepo)
    {
        _postRepo = postRepo;
        _categoryRepo = categoryRepo;
    }

    public List<Post> GetAllPosts()
    {
        return _postRepo.Select.ToList();
    }

    public async Task<Post> GetPostById(string id)
    {
        var post = await _postRepo.Select.Where(a => a.Id == id).FirstAsync();

        return post;
    }

    public async Task InsetPost(Post post)
    {
        await _postRepo.InsertAsync(post);
    }
}
