namespace AuthServerApi.Services.PasswordHasers;

public class BCryptPasswordHasher : IPasswordHasher
{
    public string HashPassword(string password)
    {
       return BCrypt.Net.BCrypt.HashPassword(password);
    }
}
