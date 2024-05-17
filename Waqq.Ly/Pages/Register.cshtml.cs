using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using Waqq.Ly.Models;


namespace Waqq.Ly.Pages
{
    public class RegisterModel : PageModel
    {
        public async Task<IActionResult> OnPostAsync(string username, string password, string name, string email, string phone, int age, bool walker)
        {
            // Validate form data (optional)
            // You can add validation logic here before sending the request

            // Create a Register object from form data
            Register registerData = new Register()
            {
                Username = username,
                Password = password,
                Name = name,
                Email = email,
                Phone = phone,
                Age = age,
                Walker = walker
            };

            // Send data to the API using HttpClient (assuming you have using statements for System.Net.Http)
            using (var client = new HttpClient())
            {
                string baseUrl = "http://api:8070"; // Replace with your actual API base URL
                string url = $"{baseUrl}/Register";

                var content = new StringContent(JsonConvert.SerializeObject(registerData), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    // Handle successful registration (e.g., display success message)
                    return RedirectToPage("/Index"); // Or another page
                }
                else
                {
                    // Handle registration error (e.g., display error message)
                    ModelState.AddModelError(string.Empty, $"Registration failed: {response.StatusCode}");
                    return Page();
                }
            }
        }
    }
}
