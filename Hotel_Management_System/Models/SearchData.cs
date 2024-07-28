using MongoDB.Bson.Serialization.Attributes;

namespace Hotel_Management_System.Models
{
    public class SearchData
    {
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public string RoomType { get; set; } = string.Empty;

    }
}
