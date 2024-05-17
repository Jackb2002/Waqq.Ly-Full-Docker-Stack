using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Waqq.Ly.Pages
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            HttpContext.Session.Remove("logged_in");
            HttpContext.Session.Remove("session");
            return Redirect(@"\Index");
        }
    }
}
