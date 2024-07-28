using Hotel_Management_System.Models;
using Hotel_Management_System.Pagination;
using Hotel_Management_System.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Hotel_Management_System.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService roomService;
        private readonly DataPagination dataPagination;
        public RoomController(IRoomService roomService)
        {
            this.roomService = roomService;
            this.dataPagination = new DataPagination();
        }

        [Route("/GetRoomsAvailability")]
        [HttpPost]
        public ObjectResult GetRoomsAvailability([FromBody] SearchData searchData, int page = 1, int pageSize = 10)
        {
            try
            {
                if (page <= 0 || pageSize <= 0)
                {
                    return BadRequest("Page and pageSize must be greater than zero.");
                }
                var rooms = roomService.GetAllRooms(searchData.RoomType);

                #region initial logic for rooms available
                //Logic to get available rooms for a given date range

                //List<Room> availableRooms = new List<Room>();
                //foreach (var item in rooms)
                //{
                //        var bookedstatus = false;
                //        var bookingCount = item.Bookings.Count();
                //        if (bookingCount > 0)
                //            for (int i = 0; i < bookingCount; i++)
                //            {
                //                if (!(item.Bookings[i].CheckInDate >= checkOutDate || item.Bookings[i].CheckOutDate <= checkInDate))
                //                {
                //                    bookedstatus = true;
                //                }
                //            }
                //        if(!bookedstatus)
                //            availableRooms.Add(item);
                //}
                #endregion

                //Simplified the logic to get available rooms
                var availableRooms = rooms.Where(room => room.Bookings == null || !room.Bookings.Any(booking =>
                        !(booking.CheckInDate >= searchData.CheckOutDate || booking.CheckOutDate <= searchData.CheckInDate))).ToList();

                //Display only required details in the api response
                List<RoomsAvailable> finalData = new List<RoomsAvailable>();
                foreach (var room in availableRooms)
                {
                    RoomsAvailable data = new RoomsAvailable();
                    data.Id = room.Id;
                    data.RoomNumber = room.RoomNumber;
                    data.FloorNumber = room.FloorNumber;
                    data.RoomType = room.RoomType;
                    data.Capacity = room.Capacity;
                    data.PricePerNight = room.PricePerNight;
                    finalData.Add(data);
                }
                var totalCount = finalData.Count();

                if (totalCount == 0)
                {
                    return NotFound("No rooms available.");
                }

                var roomAvailabilityPaginatedResponse = dataPagination.GetPaginatedResponse(finalData, page, pageSize);

                return Ok(roomAvailabilityPaginatedResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/<RoomController>
        [HttpGet]
        public ObjectResult GetRooms(int page=1, int pageSize=10)
        {
            try
            {
                if (page <= 0 || pageSize <= 0)
                {
                    return BadRequest("Page and pageSize must be greater than zero.");
                }

                var rooms = roomService.GetAllRooms("");
                var totalCount = rooms.Count();

                if (totalCount == 0)
                {
                    return NotFound("No rooms available.");
                }
                var roomDetailsPaginatedResponse = dataPagination.GetPaginatedResponse(rooms, page, pageSize);

                return Ok(roomDetailsPaginatedResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET api/<RoomController>/5
        [HttpGet("{id}")]
        public ActionResult<Room> GetSingleRoom(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return BadRequest("Room ID is required.");
                }

                var room = roomService.GetSingleRoom(id);

                if (room == null)
                {
                    return NotFound($"Room with Id={id} not found");
                }

                return Ok(room);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }

        // POST api/<RoomController>
        [HttpPost]
        public ActionResult<Room> Post([FromBody] Room newRoomDetails)
        {
            if (newRoomDetails == null)
            {
                return BadRequest("Room details cannot be null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                roomService.AddNewRoom(newRoomDetails);
                return CreatedAtAction(nameof(GetRooms), new { id = newRoomDetails.Id }, newRoomDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT api/<RoomController>/5
        [HttpPut("{id}")]
        public ActionResult UpdateRoomDetails(string id, [FromBody] Room roomData)
        {
            if (roomData == null)
            {
                return BadRequest("Room data cannot be null.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var room = roomService.GetSingleRoom(id);
                if (room == null)
                {
                    return NotFound($"Room with Id = {id} not found.");
                }

                roomService.UpdateRoomDetails(id, roomData);
                return Ok($"Room with Id = {id} updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE api/<RoomController>/5
        [HttpDelete("{id}")]
        public ActionResult DeleteRoom(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Room ID cannot be null or empty.");
            }

            try
            {
                var room = roomService.GetSingleRoom(id);
                if (room == null)
                {
                    return NotFound($"Room with ID = {id} not found.");
                }

                roomService.DeleteRoomData(id);
                return Ok($"Room with ID = {id} deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        
    }
}
