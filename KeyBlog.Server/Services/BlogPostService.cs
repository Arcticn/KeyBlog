using FreeSql;
using KeyBlog.Data.Models.DTOs;
using KeyBlog.Data.Models.Entities;
using KeyBlog.Data.Services;
using KeyBlog.Server.Services.QueryFilters;

namespace KeyBlog.Server.Services;

public class BlogPostService : PostService
{
    public BlogPostService(IBaseRepository<Post> postRepo, IBaseRepository<Category> categoryRepo) : base(postRepo, categoryRepo)
    {
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

        if(!string.IsNullOrWhiteSpace(param.Search)){
            querySet = querySet.Where(a => a.Title.Contains(param.Search));
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
