using FreeSql;
using KeyBlog.Data.Models.DTOs;
using KeyBlog.Data.Models.Entities;
using KeyBlog.Server.Services.QueryFilters;

namespace KeyBlog.Server.Services;

public class ArticleService
{
    private readonly IBaseRepository<Article> _articleRepo;
    private readonly IBaseRepository<Category> _categoryRepo;

    public ArticleService(IBaseRepository<Article> articleRepo, IBaseRepository<Category> categoryRepo)
    {
        _articleRepo = articleRepo;
        _categoryRepo = categoryRepo;
    }

    public List<Article> GetAllArticles()
    {
        return _articleRepo.Select.ToList();
    }

    public async Task<PagedResult<Article>> GetPagedList(ArticleQueryParameters param, bool adminMode = false)
    {
        var querySet = _articleRepo.Select;

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

        var pagedResult = new PagedResult<Article>
        {
            Items = items,
            PageNumber = param.Page,
            PageSize = param.PageSize,
            TotalCount = (int)totalCount
        };

        return pagedResult;
    }

    public async Task<Article> GetArticleById(string id)
    {
        var article= await _articleRepo.Select.Where(a => a.Id == id).FirstAsync();
        
        return article;
    }
}
