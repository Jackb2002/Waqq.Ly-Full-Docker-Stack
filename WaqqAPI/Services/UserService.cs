using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WaqqAPI.Models;

namespace WaqqAPI.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users; 

        public UserService(IOptions<DatabaseSettings> userStoreSettings)
        {
            var mongoClient = new MongoClient(userStoreSettings.Value.ConnectionString);
            var database = mongoClient.GetDatabase(userStoreSettings.Value.DatabaseName);

            _users = database.GetCollection<User>(userStoreSettings.Value.UsersCollectionName);
        }

        public async Task<List<User>> GetAsync() => await _users.Find(user => true).ToListAsync();
        public async Task<User> GetAsync(string id) => await _users.Find<User>(user => user.Id.ToString() == id).FirstOrDefaultAsync();
        public async Task<User> GetByUsernameAsync(string username) => await _users.Find<User>(user => user.Username.ToString() == username).FirstOrDefaultAsync();
        public async Task<User> GetByTokenAsync(string token) => await _users.Find<User>(user => user.AuthToken.ToString() == token).FirstOrDefaultAsync(); 
        public async Task<User> CreateAsync(User user)
        {
            await _users.InsertOneAsync(user);
            return user;
        }

        public async Task UpdateAsync(string id, User userIn) => await _users.ReplaceOneAsync(user => user.Id.ToString() == id, userIn);

        public async Task RemoveAsync(User userIn) => await _users.DeleteOneAsync(user => user.Id == userIn.Id);

        internal async void RenewTokenAsync(User userIn)
        {
            userIn.AuthToken = Guid.NewGuid().ToString();
            userIn.CookieTimeout = DateTime.Now.AddMinutes(30);
            await UpdateAsync(userIn.Id.ToString(), userIn);
        }
    }
}
