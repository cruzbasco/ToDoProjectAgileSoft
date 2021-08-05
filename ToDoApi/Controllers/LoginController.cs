using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using ToDoDataAccess.Models;
using ToDoDataAccess.Repositories;
using ToDoTools;
using ToDoApi.Infrastructure;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ToDoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IUserRepository _repository;
        private readonly IJwtAuthManager _jwtAuthManager;
        public LoginController(ILogger<LoginController> logger, IUserRepository repository, IJwtAuthManager jwtAuthManager)
        {
            _logger = logger;
            _repository = repository;
            _jwtAuthManager = jwtAuthManager;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login ([FromBody]LoginRequest request)
        {
            if (!ModelState.IsValid) return BadRequest();

            string encryptedPassword = Encrypt.EncryptPassword(request.Password);
            var userExist = _repository.GetUserByUsernameAndPassword(request.UserName, encryptedPassword);

            if (userExist == null) 
            {
                ModelState.AddModelError("Loging Failed", "Username or password doesn't match");
                return BadRequest(ModelState);
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Sid,userExist.Id.ToString()),
                new Claim(ClaimTypes.Name,userExist.Name)
            };

            var jwtResult = _jwtAuthManager.GenerateTokens(request.UserName, claims, DateTime.Now);
            _logger.LogInformation($"User [{request.UserName}] logged in the system.");
            return Ok(new LoginResult
            {
                Name = request.UserName,
                AccessToken = jwtResult.AccessToken,
                RefreshToken = jwtResult.RefreshToken.TokenString
            });
        }

        [HttpGet("user")]
        [Authorize]
        public ActionResult GetCurrentUser()
        {
            return Ok(new LoginResult
            {
                Name = User.Identity?.Name
            });
        }
    }

    public class LoginRequest
    {
        [Required]
        [JsonPropertyName("username")]
        public string UserName { get; set; }

        [Required]
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }

    public class LoginResult
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; }

        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }
    }

    public class RefreshTokenRequest
    {
        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }
    }
}
