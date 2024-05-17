namespace Waqq.Ly.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int Age { get; set; }
        public bool Walker { get; set; }
        public string Location { get; set; }
        public string AuthCookie { get; set; }
        public DateTime CookieTimeout { get; set; }
    }
}
