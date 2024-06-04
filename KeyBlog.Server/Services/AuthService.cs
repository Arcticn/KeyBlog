using FreeSql;
using KeyBlog.Data.Models.Entities;
using KeyBlog.Data.Services;

public class AuthService : UserService
{
    private readonly JWTHelper _jwtHelper;
    public AuthService(IBaseRepository<User> userRepo, JWTHelper jwtHelper) : base(userRepo)
    {
        _jwtHelper = jwtHelper;
    }
    public string GenerateJwtToken(User user)
    {
        return _jwtHelper.GetAccessToken(user.IsAdmin ? "Admin" : "User");
    }
}