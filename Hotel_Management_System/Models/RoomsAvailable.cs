using MongoDB.Bson.Serialization.Attributes;

namespace Hotel_Management_System.Models
{
    public class RoomsAvailable
    {
        public string Id { get; set; } = string.Empty;
        public string RoomNumber { get; set; } = string.Empty;
        public int FloorNumber { get; set; }
        public string RoomType { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public string PricePerNight { get; set; } = string.Empty;
    }
}
