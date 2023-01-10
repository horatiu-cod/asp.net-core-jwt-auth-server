namespace AuthServerApi.Services.PasswordHasers;

public interface IPasswordHasher
{
    string HashPassword(string password);
}
