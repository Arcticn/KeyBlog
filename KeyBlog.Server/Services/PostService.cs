using FreeSql;
using KeyBlog.Data.Models;
using KeyBlog.Server.Services.QueryFilters;
using KeyBlog.Server.Utils;

namespace KeyBlog.Server.Services;

public class PostService
{
    private readonly IBaseRepository<Post> _postRepo;
    private readonly IBaseRepository<Category> _categoryRepo;

    public PostService(IBaseRepository<Post> postRepo, IBaseRepository<Category> categoryRepo)
    {
        _postRepo = postRepo;
        _categoryRepo = categoryRepo;
    }

    public List<Post> GetAllPosts()
    {
        return _postRepo.Select.ToList();
    }

    public async Task<PagedResult<Post>> GetPagedList(PostQueryParameters param, bool adminMode = false)
    {
        var querySet = _postRepo.Select;

        // 筛选发布状态
        if (param.IsPublish != null && adminMode)
        {
            querySet = querySet.Where(a => a.IsPublish == param.IsPublish);
        }

        if (!adminMode)
        {
            querySet = querySet.Where(a => a.IsPublish);
        }

        // 状态过滤
        if (!string.IsNullOrWhiteSpace(param.Status))
        {
            querySet = querySet.Where(a => a.Status == param.Status);
        }

        // 分类过滤
        if (param.CategoryId != 0)
        {
            querySet = querySet.Where(a => a.CategoryId == param.CategoryId);
        }

        // 关键词过滤
        if (!string.IsNullOrWhiteSpace(param.Search))
        {
            querySet = querySet.Where(a => a.Title.Contains(param.Search));
        }

        // 排序
        if (!string.IsNullOrWhiteSpace(param.SortBy))
        {
            var isAscending = !param.SortBy.StartsWith("-");
            var orderByProperty = param.SortBy.Trim('-');
            querySet = querySet.OrderByPropertyName(orderByProperty, isAscending);
        }

        var totalCount = await querySet.CountAsync();
        var items = await querySet.Page(param.Page, param.PageSize).Include(a => a.Category).ToListAsync();

        var pagedResult = new PagedResult<Post>
        {
            Items = items,
            PageNumber = param.Page,
            PageSize = param.PageSize,
            TotalCount = (int)totalCount
        };

        return pagedResult;
    }
}
