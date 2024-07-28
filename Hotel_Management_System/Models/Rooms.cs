using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Hotel_Management_System.Models
{
    [BsonIgnoreExtraElements]
    public class Room
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        [BsonElement("roomNumber")]
        public string RoomNumber { get; set; } = string.Empty;
        [BsonElement("floorNumber")]
        public int FloorNumber { get; set; }
        [BsonElement("roomType")]
        public string RoomType { get; set; } = string.Empty;
        [BsonElement("capacity")]
        public int Capacity { get; set; }
        [BsonElement("bookings")]
        public List<Booking> ?Bookings { get; set; }
        [BsonElement("pricePerNight")]
        public string PricePerNight { get; set; } = string.Empty;
    }

    public class Booking
    {
        [BsonElement("checkInDate")]
        public DateTime CheckInDate { get; set; }
        [BsonElement("checkOutDate")]

        public DateTime CheckOutDate { get; set; }
    }
}
