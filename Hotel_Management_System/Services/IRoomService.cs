using Hotel_Management_System.Models;

namespace Hotel_Management_System.Services
{
    public interface IRoomService
    {
        List<Room> GetAllRooms(string roomType);
        Room GetSingleRoom(string id);
        Room AddNewRoom(Room roomDetails);
        void UpdateRoomDetails(string id, Room roomDetails);
        void DeleteRoomData(string id);
    }
}
