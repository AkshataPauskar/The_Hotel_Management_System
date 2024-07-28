using Hotel_Management_System.Models;
using MongoDB.Driver;

namespace Hotel_Management_System.Services
{
    public class RoomService : IRoomService
    {
        private readonly IMongoCollection<Room> _rooms;

        public RoomService(IHotelManagementDatabaseSettings settings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _rooms = database.GetCollection<Room>("rooms");
        }
        public Room AddNewRoom(Room roomDetails)
        {
            _rooms.InsertOne(roomDetails);
            return roomDetails;
        }

        public void DeleteRoomData(string id)
        {
            _rooms.DeleteOne(room => room.Id == id);
        }

        public List<Room> GetAllRooms(string roomType="")
        {
            if(roomType == "")
                return _rooms.Find(room => true).ToList();
            else
                return _rooms.Find(room=>room.RoomType==roomType).ToList();
        }

        public Room GetSingleRoom(string id)
        {
            return _rooms.Find(room => room.Id == id).FirstOrDefault();
        }

        public void UpdateRoomDetails(string id, Room roomDetails)
        {
            _rooms.ReplaceOne(room => room.Id == id, roomDetails);
        }
    }
}
