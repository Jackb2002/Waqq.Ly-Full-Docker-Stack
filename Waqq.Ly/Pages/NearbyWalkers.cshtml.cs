using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text;
using Waqq.Ly.Models;

namespace Waqq.Ly.Pages
{
    public class NearbyWalkersModel : PageModel
    {
        public List<string> Walkers = new List<string>();
        public void OnGet()
        {
            if(HttpContext.Session.GetInt32("logged_in") != 1)
            {
                Redirect("/Login");
            }

            string api = API.access_address;
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
                        string location = lines[6].Split(":")[1];
                        GetWalkers(api, client, out _, out _, out _, location);
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

        private void GetWalkers(string api, HttpClient client, out string url, out StringContent content, out HttpResponseMessage response, string location)
        {
            WalkersRequest walkersRequest = new WalkersRequest()
            {
                Location = location
            };
            content = new StringContent(JsonConvert.SerializeObject(walkersRequest), Encoding.UTF8, "application/json");
            url = api + "/NearbyWalkers";
            response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                Walkers = response.Content.ReadAsStringAsync().Result.Split("\n").ToList();
            }
            else
            {
                Walkers.Append("No walkers found");
            }
        }
    }
}
