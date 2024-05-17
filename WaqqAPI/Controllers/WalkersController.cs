using Microsoft.AspNetCore.Mvc;
using System.Text;
using WaqqAPI.Services;

namespace WaqqAPI.Controllers
{
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

                var users = await _userService.GetAsync();
                foreach (var user in users)
                {
                    if (user.Location.ToLower().Trim() == location.ToLower().Trim())
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
