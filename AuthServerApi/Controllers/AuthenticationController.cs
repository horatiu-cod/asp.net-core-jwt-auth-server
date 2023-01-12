using AuthServerApi.Models;
using AuthServerApi.Models.Requests;
using AuthServerApi.Models.Responses;
using AuthServerApi.Services.PasswordHasers;
using AuthServerApi.Services.TokenGenerators;
using AuthServerApi.Services.UserRepositories;
using Microsoft.AspNetCore.Mvc;

namespace AuthServerApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly IUserRepositories _userrepositories;
        private readonly IPasswordHasher _passwordHasher;
        private readonly AccesTokenGenerator _accesTokenGenerator;

        public AuthenticationController(IUserRepositories userrepositories, IPasswordHasher passwordHasher, AccesTokenGenerator accesTokenGenerator)
        {
            _userrepositories = userrepositories;
            _passwordHasher = passwordHasher;
            _accesTokenGenerator = accesTokenGenerator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            if (registerRequest.Password != registerRequest.ConfirmPassword)
            {
                return BadRequest(new ErrorResponse("Password does not mach confirm password"));
            }

            User existingUserByEmail = await _userrepositories.GetByEmail(registerRequest.Email);
            if (existingUserByEmail != null)
            {
                return Conflict(new ErrorResponse("Email already exist"));
            }

            User existingUserByUsername = await _userrepositories.GetByUserName(registerRequest.UserName);
            if (existingUserByUsername != null)
            {
                return Conflict(new ErrorResponse("User already exist"));
            }

            string passwordHash = _passwordHasher.HashPassword(registerRequest.Password);
            User registrationUser = new User()
            {
                Email = registerRequest.Email,
                Username = registerRequest.UserName,
                PasswordHash = passwordHash
            };

            await _userrepositories.CreateUser(registrationUser);

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                BadRequestModelState();
            }

            User existingUserByName = await _userrepositories.GetByUserName(loginRequest.UserName);
            if (existingUserByName is null)
            {
                return Unauthorized(new ErrorResponse("User does not exist"));
            }


            var isCorrectPassword = _passwordHasher.VerifyPassword(loginRequest.Password, existingUserByName.PasswordHash);
            if (!isCorrectPassword)
            {
                return Unauthorized(new ErrorResponse("Password not valid"));
            }

            string accesToken = _accesTokenGenerator.GenerateToken(existingUserByName);

            return Ok(new AuthenticatedUserResponse() 
            { 
                AccesToken = accesToken 
            
            });

        }

        private IActionResult BadRequestModelState()
        {
            IEnumerable<string> errorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));

            return BadRequest(new ErrorResponse(errorMessages));
        }
    }
}
