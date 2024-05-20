using FreeSql;
using KeyBlog.Data.Models;

namespace KeyBlog.Server.Services;

public class CategoryService
{
    private readonly IBaseRepository<Category> _cRepo;

    public CategoryService(IBaseRepository<Category> cRepo)
    {
        _cRepo = cRepo;
    }


    //_categoryRepo.Where(a => a.Id == categoryId)
    public Task<Category> GetCategory(int categoryId)
    {
        return _cRepo.Where(a => a.Id == categoryId).FirstAsync();
    }

    /*internal async Task GetNodes()
    {
        throw new NotImplementedException();
    }*/
}