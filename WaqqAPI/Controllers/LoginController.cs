using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using WaqqAPI.RequestModels;
using WaqqAPI.Services;
using WaqqAPI.Models;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    private readonly ILogger<LoginController> _logger;
    private readonly UserService _userService;

    public LoginController(ILogger<LoginController> logger, UserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    [HttpPost(Name = "UserLogin")]
    public async Task<IActionResult> UserLogin([FromBody] LoginRequest loginData)
    {
        if (loginData == null)
        {
            return BadRequest("Invalid login data");
        }

        string username = loginData.Username;
        _logger.LogInformation($"User logging in: {username}");

        User User = _userService.GetByUsernameAsync(username).Result;
        if(User == null)
        {
            _logger.LogWarning("User not found: " + username);
            return NotFound("User not found");
        }
        if(User.Password != loginData.Password)
        {
            _logger.LogWarning("Incorrect password attempt for " + User.Username);
            return Unauthorized("Invalid password");
        }
        User.AuthToken = Guid.NewGuid().ToString();
        _logger.LogInformation("Logged in user" + User.Username);
        User.CookieTimeout = DateTime.Now.AddMinutes(30);
        await _userService.UpdateAsync(User.Id.ToString(), User); // Update for token to reflect into db
        _logger.LogInformation("user valid token " + User.AuthToken);
        return Ok(User.AuthToken.ToString());
    }
}
