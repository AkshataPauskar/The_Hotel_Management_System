using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Hotel_Management_System.Models
{
    [BsonIgnoreExtraElements]
    public class Reservation
    {
            [BsonId]
            [BsonRepresentation(BsonType.ObjectId)]
            public string Id { get; set; } = string.Empty;
            [BsonElement("bookingName")]
            public string BookingName { get; set; } = string.Empty;
            [BsonElement("mailId")]
            public string MailId { get; set; } = string.Empty;
            [BsonElement("contactNumber")]
            public string ContactNumber { get; set; } = string.Empty;
            [BsonElement("address")]
            public string Address { get; set; } = string.Empty;
            [BsonElement("fromDate")]
            public DateTime FromDate { get; set; }
            [BsonElement("toDate")]
            public DateTime ToDate { get; set; }
            [BsonElement("totalNumberOfDays")]
            public int TotalNumberOfDays { get; set; }
            [BsonElement("roomNumber")]
            public string RoomNumber { get; set; } = string.Empty;
            [BsonElement("rentPerDay")]
            public string RentPerDay { get; set; } = string.Empty;
        [BsonElement("totalAmount")]
            public string TotalAmount { get; set; } = string.Empty;
        [BsonElement("paymentStatus")]
            public string PaymentStatus { get; set; } = string.Empty;
        [BsonElement("bookingStatus")]
            public string BookingStatus { get; set; } = string.Empty;

    }
}
