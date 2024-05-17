using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using System.Net.Http.Headers;
using System.Text;
using Waqq.Ly.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Waqq.Ly.Pages
{
    public class ProfileModel : PageModel
    {

        public async void OnGet()
        {
            if(HttpContext.Session.GetInt32("logged_in") != 1)
            {
                Redirect("/Login");
            }

            string api = "http://api:8070";
            string endpoint = "/Profile";
            string? auth_key = HttpContext.Session.GetString("session") ?? "";
            using (HttpClient client = new HttpClient())
            {
                string url = api + endpoint;
                TokenRequest token_data = new TokenRequest()
                {
                    token = auth_key,
                    time = DateTime.Now
                };

                var content = new StringContent(JsonConvert.SerializeObject(token_data), Encoding.UTF8, "application/json");
                var response = client.PostAsync(url, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    if (data == "Invalid token")
                    {
                        HttpContext.Session.Remove("session");
                        Redirect("/Login");
                    }
                    else if (data == "Token expired")
                    {
                        HttpContext.Session.Remove("session");
                        Redirect("/Login");
                    }
                    else
                    {
                        string[] lines = data.Split("\n");
                        ViewData["Username"] = lines[0].Split(":")[1];
                        ViewData["Email"] = lines[1].Split(":")[1];
                        ViewData["Name"] = lines[2].Split(":")[1];
                        ViewData["Phone"] = lines[3].Split(":")[1];
                        ViewData["Age"] = lines[4].Split(":")[1];
                        ViewData["Walker"] = lines[5].Split(":")[1];
                    }
                }
                else
                {
                    try
                    {
                        HttpContext.Session.Remove("session");
                        HttpContext.Session.Remove("logged_in");
                    }
                    catch 
                    {
                        Redirect("/Index");
                    }
                    Redirect("/Login");
                }
            }
        }
    }
}
