using Hotel_Management_System.Models;
using MongoDB.Driver;

namespace Hotel_Management_System.Services
{
    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> _users;
        public UserService(IHotelManagementDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            //_users=database.GetCollection<User>(settings.CollectionName);
            _users = database.GetCollection<User>("users");
        }
        public User Create(User user)
        {
            _users.InsertOne(user);
            return user;
        }

        public void Delete(string id)
        {
            _users.DeleteOne(user => user.Id == id);
        }

        public User GetUser(string id)
        {
            return _users.Find(user => user.Id == id).FirstOrDefault();
        }

        public List<User> GetUsers()
        {
            return _users.Find(user=>true).ToList();
        }

        public void Update(string id, User user)
        {
            _users.ReplaceOne(user => user.Id == id, user);
        }

        public User ValidateUser(Login loginDetails)
        {
           return _users.Find(user=>user.UserName==loginDetails.UserName && user.Password==loginDetails.Password).FirstOrDefault();
        }
    }
}
