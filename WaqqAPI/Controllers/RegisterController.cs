using Microsoft.AspNetCore.Mvc;
using WaqqAPI.Models;
using WaqqAPI.RequestModels;
using WaqqAPI.Services;

namespace WaqqAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegisterController : ControllerBase
    {
        private readonly ILogger<RegisterController> _logger;
        private readonly UserService _userService;


        public RegisterController(ILogger<RegisterController> logger, UserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpPost(Name = "UserRegister")]
        public async Task<IActionResult> UserRegister([FromBody] RegisterRequest registerData)
        {
            if (registerData == null)
            {
                return BadRequest("Invalid registration data");
            }

            // Currently, this code stores the data in the registerData object
            // You'll need to implement database logic to persist this data

            _logger.LogInformation($"User register query: {registerData.Username}");
            var newUser = new User
            {
                Username = registerData.Username,
                Password = registerData.Password,
                Email = registerData.Email,
                Name = registerData.Name,
                Phone = registerData.Phone,
                Age = registerData.Age,
                Walker = registerData.Walker,
                AuthToken = "",
                CookieTimeout = DateTime.Now,
                Location = registerData.Location
            };

            await _userService.CreateAsync(newUser);

            return CreatedAtAction("UserRegister", newUser);
        }
    }
}
