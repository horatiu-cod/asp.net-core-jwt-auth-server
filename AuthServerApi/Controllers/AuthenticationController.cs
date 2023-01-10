using AuthServerApi.Models;
using AuthServerApi.Models.Requests;
using AuthServerApi.Models.Responses;
using AuthServerApi.Services.PasswordHasers;
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

        public AuthenticationController(IUserRepositories userrepositories, IPasswordHasher passwordHasher)
        {
            _userrepositories = userrepositories;
            _passwordHasher = passwordHasher;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<string> errorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));

                return BadRequest(new ErrorResponse(errorMessages));
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
    }
}
