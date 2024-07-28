using Hotel_Management_System.Models;

namespace Hotel_Management_System.Services
{
    public interface IUserService
    {
        List<User> GetUsers();
        User GetUser(string id);
        User Create(User user);
        void Update (string id, User user);
        void Delete (string id);
        User ValidateUser(Login loginDetails);
    }
}
