using MongoDB.Bson.Serialization.Attributes;

namespace Hotel_Management_System.Models
{
    public class Login
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
