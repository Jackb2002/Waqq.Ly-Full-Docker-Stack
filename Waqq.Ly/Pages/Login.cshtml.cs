using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net;
using System.Security.Claims;
using System.Text;
using Waqq.Ly.Models; 

namespace Waqq.Ly.Pages
{
    public class LoginModel : PageModel
    {
        public IActionResult OnGet()
        {
            if(HttpContext.Session.GetInt32("logged_in") == 1)
            {
                return RedirectToPage("/Index");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string username, string password)
        {
            if (!ModelState.IsValid) // Validate form data (optional)
            {
                return Page(); // Return the page with validation errors
            }

            // Create Login object from form data
            Login loginData = new Login()
            {
                Username = username,
                Password = password // Hash password before sending (discussed later)
            };

            // Send data to the API using HttpClient (assuming you have using statements for System.Net.Http)
            using (var client = new HttpClient())
            {
                string baseUrl = "http://api:8070"; 
                string url = $"{baseUrl}/Login";

                var content = new StringContent(JsonConvert.SerializeObject(loginData), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    string auth_token = response.Content.ReadAsStringAsync().Result;
                    return HandleLoginSuccess(response,auth_token);
                }
                else if(response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    ModelState.AddModelError(string.Empty, "Password is incorrect");
                    return Page();
                }
                else if(response.StatusCode == HttpStatusCode.NotFound)
                {
                    ModelState.AddModelError(string.Empty, "User not found " + $"\"{username}\"");
                    return Page();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, $"Login failed: {response.StatusCode}");
                    return Page();
                }
            }
        }

        private IActionResult HandleLoginSuccess(HttpResponseMessage response, string auth_token)
        {
            HttpContext.Session.SetString("session", auth_token);
            HttpContext.Session.SetInt32("logged_in", 1);
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                ModelState.AddModelError(string.Empty, $"Login success: {auth_token}");
                ModelState.AddModelError(string.Empty, $"Login success: {HttpContext.Session.GetString("session")}");
                return Page();
            }
            else
            {
                return RedirectToPage("/Index"); // Or another page
            }
        }
    }

}
