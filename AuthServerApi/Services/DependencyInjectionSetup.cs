using AuthServerApi.Models;
using AuthServerApi.Services.PasswordHasers;
using AuthServerApi.Services.TokenGenerators;
using AuthServerApi.Services.UserRepositories;

namespace AuthServerApi.Services;

public static class DependencyInjectionSetup
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddControllers();
        services.AddSingleton<IPasswordHasher, BCryptPasswordHasher>();
        services.AddSingleton<IUserRepositories, InMemoryUserRepository>();
        services.AddSingleton<AccesTokenGenerator>();

        return services;
    }
}
