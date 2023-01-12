using AuthServerApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthServerApi.Services.TokenGenerators;

public class AccesTokenGenerator
{
    private readonly AuthenticationConfiguration _configuration;

    public AccesTokenGenerator(AuthenticationConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(User user)
    {
        SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.AccesTokenSecret));
        SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        List<Claim> claims = new()
        {
            new Claim("id", user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Username)
        };
        JwtSecurityToken token = new(
            _configuration.Issuer, 
            _configuration.Audience, 
            claims,
            DateTime.UtcNow,
            DateTime.UtcNow.AddMinutes(_configuration.AccesTokenExpirationMinutes),
            credentials
            );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
