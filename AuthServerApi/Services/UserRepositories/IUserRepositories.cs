using AuthServerApi.Models;

namespace AuthServerApi.Services.UserRepositories;

public interface IUserRepositories
{
    Task<User> GetByEmail(string email);
    Task<User> GetByUserName(string userName);

    Task<User> CreateUser(User user);
}
