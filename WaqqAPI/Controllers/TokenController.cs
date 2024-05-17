using Microsoft.AspNetCore.Mvc;
using WaqqAPI.RequestModels;
using WaqqAPI.Services;
using WaqqAPI.Models;


[ApiController]
[Route("[controller]")]
public class TokenController : ControllerBase
{
    private readonly ILogger<LoginController> _logger;
    private readonly UserService _userService;

    public TokenController(ILogger<LoginController> logger, UserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    [HttpPost(Name = "TokenCheck")]
    public async Task<IActionResult> TokenCheck([FromBody] TokenRequest tokenData)
    {
        if (tokenData == null)
        {
            return BadRequest("Invalid token data");
        }

        return await ValidateToken(tokenData);
    }

    private async Task<IActionResult> ValidateToken(TokenRequest tokenData)
    {
        string token = tokenData.token;
        _logger.LogInformation($"Checking token: {token}");

        User User = await _userService.GetByTokenAsync(token);
        if (User == null)
        {
            return NotFound("User not found");
        }
        if (User.AuthToken != token)
        {
            return Unauthorized("Invalid token");
        }
        if (User.CookieTimeout < DateTime.Now)
        {
            return Unauthorized("Token expired");
        }
        return Ok(User.Username);
    }
}

