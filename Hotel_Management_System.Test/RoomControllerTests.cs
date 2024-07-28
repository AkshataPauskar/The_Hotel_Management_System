using Hotel_Management_System.Controllers;
using Hotel_Management_System.Models;
using Hotel_Management_System.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Hotel_Management_System.Test
{
    public class RoomControllerTests
    {
        private readonly Mock<IRoomService> _mockRoomService;
        private readonly RoomController _roomController;

        public RoomControllerTests()
        {
            _mockRoomService = new Mock<IRoomService>();
            _roomController = new RoomController(_mockRoomService.Object);
        }

        [Fact]
        public void GetRooms_ReturnsPaginatedListOfRooms()
        {
            // Arrange
            var rooms = new List<Room>
        {
            new Room { Id = "1", RoomNumber = "101" ,RoomType="Single",FloorNumber=1,Capacity=2,Bookings=[],PricePerNight="1000"},
            new Room { Id = "2", RoomNumber = "102" ,RoomType="Double",FloorNumber=1,Capacity=2,Bookings=[],PricePerNight="1500"},
            new Room { Id = "3", RoomNumber = "103" ,RoomType="Deluxe",FloorNumber=1,Capacity=2,Bookings=[],PricePerNight="2000"},
            new Room { Id = "4", RoomNumber = "104" ,RoomType="Family",FloorNumber=1,Capacity=4,Bookings=[],PricePerNight="2500"},
            new Room { Id = "5", RoomNumber = "105" ,RoomType="Single",FloorNumber=1,Capacity=2,Bookings=[],PricePerNight="1000"},
            new Room { Id = "6", RoomNumber = "106" ,RoomType="Single",FloorNumber=1,Capacity=2,Bookings=[],PricePerNight="1000"},
            new Room { Id = "7", RoomNumber = "107" ,RoomType="Deluxe",FloorNumber=1,Capacity=2,Bookings=[],PricePerNight="2000"},
            new Room { Id = "8", RoomNumber = "108" ,RoomType="Single",FloorNumber=1,Capacity=2,Bookings=[],PricePerNight="1000"},
            new Room { Id = "9", RoomNumber = "109" ,RoomType="Family",FloorNumber=1,Capacity=4,Bookings=[],PricePerNight="2500"},
            new Room { Id = "1", RoomNumber = "110",RoomType="Single",FloorNumber=1,Capacity=2,Bookings=[],PricePerNight="1000" },
            new Room { Id = "1", RoomNumber = "111",RoomType="Double",FloorNumber=1,Capacity=2,Bookings=[],PricePerNight="1500" }
        };

            _mockRoomService.Setup(service => service.GetAllRooms(It.IsAny<string>())).Returns(rooms);

            var page = 1;
            var pageSize = 10;

            // Act
            var result = _roomController.GetRooms(page, pageSize) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            var paginatedResponse = Assert.IsType<PaginatedResponse<Room>>(result.Value);
            Assert.Equal(10, paginatedResponse.Data.Count);
            Assert.Equal(11, paginatedResponse.TotalCount);
            Assert.Equal(2, paginatedResponse.TotalPages);
            Assert.Equal(1, paginatedResponse.CurrentPage);
            Assert.Equal(10, paginatedResponse.PageSize);
        }


        [Fact]
        public void GetRooms_ReturnsNotFoundIfNoRooms()
        {
            // Arrange
            var rooms = new List<Room>();
            _mockRoomService.Setup(service => service.GetAllRooms(It.IsAny<string>())).Returns(rooms);

            var page = 1;
            var pageSize = 10;

            // Act
            var result = _roomController.GetRooms(page, pageSize);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void GetRooms_ReturnsBadRequest_IfPageExceedsTotalPages()
        {
            // Arrange
            var rooms = new List<Room> {             
                new Room { Id = "1", RoomNumber = "101" ,RoomType="Single",FloorNumber=1,Capacity=2,Bookings=[],PricePerNight="1000"} };
            
            _mockRoomService.Setup(service => service.GetAllRooms(It.IsAny<string>())).Returns(rooms);

            // Act
            var result = _roomController.GetRooms(2, 10);

            // Assert
            Assert.NotNull(result);
            var paginatedResponse = Assert.IsType<PaginatedResponse<Room>>(result.Value);
            Assert.Empty(paginatedResponse.Data);
            Assert.Equal(1, paginatedResponse.TotalCount);
            Assert.Equal(1, paginatedResponse.TotalPages);
        }

        [Fact]
        public void GetRooms_ReturnsBadRequest_ForInvalidPageOrPageSize()
        {
            // Arrange
            _mockRoomService.Setup(service => service.GetAllRooms(It.IsAny<string>())).Returns(new List<Room>());

            // Act
            var result1 = _roomController.GetRooms(0, 10);
            var result2 = _roomController.GetRooms(1, 0);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result1);
            Assert.IsType<BadRequestObjectResult>(result2);
        }

        //RoomAvaliability api unit test cases
        [Fact]
        public void GetRoomsAvailability_ReturnsBadRequest_ForInvalidPageOrPageSize()
        {
            var searchData = new SearchData { CheckInDate = DateTime.Now, CheckOutDate = DateTime.Now.AddDays(1), RoomType = "Deluxe" };
            // Act
            var result1 = _roomController.GetRoomsAvailability(searchData, 0, 10);
            var result2 = _roomController.GetRoomsAvailability(searchData, 1, 0);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result1);
            Assert.IsType<BadRequestObjectResult>(result2);
        }

        [Fact]
        public void GetRoomsAvailability_ReturnsBadRequest_IfPageExceedsTotalPages()
        {
            // Arrange
            var searchData = new SearchData { CheckInDate = DateTime.Now, CheckOutDate = DateTime.Now.AddDays(1), RoomType = "Deluxe" };
            var rooms = new List<Room> { 
                new Room { Id = "1", RoomNumber = "101" ,RoomType="Single",FloorNumber=1,Capacity=2,Bookings=[],PricePerNight="1000"},
               };
            _mockRoomService.Setup(service => service.GetAllRooms(It.IsAny<string>())).Returns(rooms);

            // Act
            var result = _roomController.GetRoomsAvailability(searchData, 2, 10);

            // Assert
            Assert.NotNull(result);
            var paginatedResponse = Assert.IsType<PaginatedResponse<RoomsAvailable>>(result.Value);
            Assert.Empty(paginatedResponse.Data);
            Assert.Equal(1, paginatedResponse.TotalCount);
            Assert.Equal(1, paginatedResponse.TotalPages);
        }

        [Fact]
        public void GetRoomsAvailability_ReturnsAvailableRooms()
        {
            // Arrange
            var searchData = new SearchData { CheckInDate = DateTime.Now, CheckOutDate = DateTime.Now.AddDays(1), RoomType = "" };
            var rooms = new List<Room>
            {
            new Room
            {
                Id = "1", 
                RoomNumber = "101" ,
                RoomType="Single",
                FloorNumber=1,
                Capacity=2,
                Bookings = new List<Booking>
                {
                    new Booking { CheckInDate = DateTime.Now.AddDays(5), CheckOutDate = DateTime.Now.AddDays(10) },
                    new Booking { CheckInDate = DateTime.Now.AddDays(10), CheckOutDate = DateTime.Now.AddDays(20) }

                },
                PricePerNight="1000"
            },
            new Room
            {
                Id = "2",
                RoomNumber = "102" ,
                RoomType="Deluxe",
                FloorNumber=1,
                Capacity=2,
                Bookings = new List<Booking>
                {
                    new Booking { CheckInDate = DateTime.Now.AddDays(-1), CheckOutDate = DateTime.Now.AddDays(1) }
                },
                PricePerNight="2000"
            },
            new Room { 
                Id = "3",
                RoomNumber = "103" ,
                RoomType="Double",
                FloorNumber=1,
                Capacity=2, 
                Bookings = [] ,
                PricePerNight="1500"}
            };
            _mockRoomService.Setup(service => service.GetAllRooms(It.IsAny<string>())).Returns(rooms);

            var checkInDate = DateTime.Now;
            var checkOutDate = DateTime.Now.AddDays(2);

            // Act
            var result = _roomController.GetRoomsAvailability(searchData, 1, 10) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            var paginatedResponse = Assert.IsType<PaginatedResponse<RoomsAvailable>>(result.Value);
            Assert.Equal(2, paginatedResponse.Data.Count);
            Assert.Equal(2, paginatedResponse.TotalCount);
            Assert.Equal(1, paginatedResponse.TotalPages);
            Assert.Equal(1, paginatedResponse.CurrentPage);
            Assert.Equal(10, paginatedResponse.PageSize);
        }
    }
}