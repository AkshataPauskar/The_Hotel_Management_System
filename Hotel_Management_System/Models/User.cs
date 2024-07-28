using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace Hotel_Management_System.Models
{
    [BsonIgnoreExtraElements]
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        [BsonElement("emailId")]
        public string EmailId { get; set; } = string.Empty;
        [BsonElement("username")]
        public string UserName{ get; set; } = string.Empty;
        [BsonElement("password")]
        public string Password { get; set; } = string.Empty;
        [BsonElement("isadmin")]
        public bool IsAdmin { get; set; }
        [BsonElement("dtCreatedOn")]
        public DateTime CreatedOn { get; set; }

    }
}
