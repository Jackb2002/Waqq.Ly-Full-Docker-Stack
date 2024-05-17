using Microsoft.AspNetCore.Mvc;
using System.Text;
using WaqqAPI.RequestModels;
using WaqqAPI.Services;

namespace WaqqAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly ILogger<RegisterController> _logger;
        private readonly UserService _userService;


        public ProfileController(ILogger<RegisterController> logger, UserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpPost(Name = "GetProfile")]
        public async Task<IActionResult> GetProfile([FromBody] TokenRequest token_request)
        {
            string auth_key = token_request.token;
            var user = await _userService.GetByTokenAsync(auth_key);
            if(user == null)
            {
                _logger.LogInformation("Invalid token: " + auth_key);
                return BadRequest("Invalid token");
            }
            if(user.CookieTimeout < DateTime.Now)
            {
                _logger.LogInformation("Token expired for " + user.AuthToken + ":" + user.Username);
                return BadRequest("Token expired");
            }

            _userService.RenewTokenAsync(user);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Username: " + user.Username);
            sb.AppendLine("Email: " + user.Email);
            sb.AppendLine("Name: " + user.Name);
            sb.AppendLine("Phone Number:" + user.Phone);
            sb.AppendLine("Age:" + user.Age);
            sb.AppendLine("Walker:" + user.Walker);
            sb.AppendLine("Location:" + user.Location);
            return Ok(sb.ToString());
        }
    }
}
