using Microsoft.AspNetCore.Mvc;
using System.Text;
using WaqqAPI.RequestModels;
using WaqqAPI.Services;

namespace WaqqAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalkersController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly UserService _userService;

        public WalkersController(ILogger<LoginController> logger, UserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpPost(Name = "LocateWalkers")]
        public async Task<IActionResult> LocateWalkers([FromBody] WalkersRequest walkersRequest)
        {
            try
            {
                var location = walkersRequest.Location;
                List<string> ValidWalkers = new List<string>();
                _logger.LogInformation("Looking for valid walkers in " + location);
                var users = await _userService.GetAsync();
                _logger.LogInformation("Checking " + users.Count + " users");
                foreach (var user in users)
                {
                    if (user.Location.ToLower().Trim() == location.ToLower().Trim() && user.Walker == "Yes")
                    {
                        ValidWalkers.Add($"{user.Name} - {user.Phone}");
                    }
                }

                StringBuilder sb = new StringBuilder();
                foreach (var walker in ValidWalkers)
                {
                    sb.AppendLine(walker);
                }
                _logger.LogInformation("Found walkers at " + location);
                return Ok(sb.ToString());
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
