using AuthServerApi.Models;

namespace AuthServerApi.Services.UserRepositories;

public class InMemoryUserRepository : IUserRepositories
{
    private readonly List<User> _user = new();
    public Task<User> CreateUser(User user)
    {
        user.Id = Guid.NewGuid();
        _user.Add(user);

        return Task.FromResult(user);
    }

    public Task<User> GetByEmail(string email)
    {
        return Task.FromResult(_user.FirstOrDefault(u => u.Email == email));
    }

    public Task<User> GetByUserName(string userName)
    {
        return Task.FromResult(_user.FirstOrDefault(u => u.Username == userName));
    }
}
