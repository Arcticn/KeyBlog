using KeyBlog.Data.Models.Entities;
using FreeSql;

namespace KeyBlog.Data.Services;
public class UserService
{
    protected readonly IBaseRepository<User> _userRepo;
    public UserService(IBaseRepository<User> userRepo)
    {
        _userRepo = userRepo;
    }
    public List<User> GetAllUsers()
    {
        return _userRepo.Select.ToList();
    }
    public async Task<User> GetUser(string username)
    {
        var user = await _userRepo.Select.Where(a => a.Username == username).FirstAsync();
        return user;
    }
    public async Task InsertUser(User user)
    {
        await _userRepo.InsertAsync(user);
    }
    public async Task EditUser(User user)
    {
        var tempUser = await _userRepo.Where(a => a.Id == user.Id).FirstAsync();
        user.Id = tempUser.Id;
        await _userRepo.UpdateAsync(user);
    }
    public async Task DeleteUser(int id)
    {
        await _userRepo.DeleteAsync(a => a.Id == id);
    }
}