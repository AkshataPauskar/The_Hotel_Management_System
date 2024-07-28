using Hotel_Management_System.Controllers;
using Hotel_Management_System.Models;
using Hotel_Management_System.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_Management_System.Test
{
    public class ReservationsControllerTests
    {
        private readonly Mock<IReservationsService> _mockReservationsService;
        private readonly ReservationController _reservationController;

        public ReservationsControllerTests()
        {
            _mockReservationsService = new Mock<IReservationsService>();
            _reservationController = new ReservationController(_mockReservationsService.Object);
        }

        [Fact]
        public void GetAllReservations_ReturnsBadRequest_ForInvalidPageOrPageSize()
        {
            // Act
            var result1 = _reservationController.GetAllReservations(0, 10);
            var result2 = _reservationController.GetAllReservations(1, 0);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result1);
            Assert.IsType<BadRequestObjectResult>(result2);
        }

        [Fact]
        public void GetAllReservations_ReturnsEmptyPaginatedResponse_IfPageExceedsTotalPages()
        {
            // Arrange
            var reservations = new List<Reservation> {
                new Reservation {
                    Id = "1",
                    BookingName = "Test",
                    MailId = "test@gmail.com",
                    ContactNumber = "12345678",
                    Address = "test test test",
                    FromDate = DateTime.Now,
                    ToDate = DateTime.Now.AddDays(1),
                    TotalNumberOfDays = 1,
                    RoomNumber = "101",
                    RentPerDay = "1000",
                    TotalAmount = "1000",
                    PaymentStatus = "Pending",
                    BookingStatus = "Confirmed"
                }
            };

            _mockReservationsService.Setup(service => service.GetAllReservations()).Returns(reservations);

            // Act
            var result = _reservationController.GetAllReservations(2, 10) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var paginatedResponse = Assert.IsType<PaginatedResponse<Reservation>>(result.Value);
            Assert.Empty(paginatedResponse.Data);
            Assert.Equal(1, paginatedResponse.TotalCount);
            Assert.Equal(1, paginatedResponse.TotalPages);
        }

        [Fact]
        public void GetAllReservations_ReturnsPaginatedResponse()
        {
            // Arrange
            var reservations = new List<Reservation>
            {
                new Reservation
                {
                    Id = "1",
                    BookingName = "Test1",
                    MailId = "test@gmail.com",
                    ContactNumber = "12345678",
                    Address = "test test test",
                    FromDate = DateTime.Now,
                    ToDate = DateTime.Now.AddDays(1),
                    TotalNumberOfDays = 1,
                    RoomNumber = "101",
                    RentPerDay = "1000",
                    TotalAmount = "1000",
                    PaymentStatus = "Pending",
                    BookingStatus = "Confirmed"
                },
                new Reservation
                {
                    Id = "2",
                    BookingName = "Test2",
                    MailId = "test@gmail.com",
                    ContactNumber = "12345678",
                    Address = "test test test",
                    FromDate = DateTime.Now,
                    ToDate = DateTime.Now.AddDays(4),
                    TotalNumberOfDays = 4,
                    RoomNumber = "102",
                    RentPerDay = "1000",
                    TotalAmount = "4000",
                    PaymentStatus = "Pending",
                    BookingStatus = "Confirmed"
                },
                new Reservation
                {
                    Id = "3",
                    BookingName = "Test3",
                    MailId = "test@gmail.com",
                    ContactNumber = "12345678",
                    Address = "test test test",
                    FromDate = DateTime.Now.AddDays(4),
                    ToDate = DateTime.Now.AddDays(6),
                    TotalNumberOfDays = 2,
                    RoomNumber = "103",
                    RentPerDay = "1000",
                    TotalAmount = "2000",
                    PaymentStatus = "Pending",
                    BookingStatus = "Confirmed"
                }
            };
            _mockReservationsService.Setup(service => service.GetAllReservations()).Returns(reservations);

            // Act
            var result = _reservationController.GetAllReservations(1, 2) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var paginatedResponse = Assert.IsType<PaginatedResponse<Reservation>>(result.Value);
            Assert.Equal(2, paginatedResponse.Data.Count);
            Assert.Equal(3, paginatedResponse.TotalCount);
            Assert.Equal(2, paginatedResponse.TotalPages);
            Assert.Equal(1, paginatedResponse.CurrentPage);
            Assert.Equal(2, paginatedResponse.PageSize);
        }

        [Fact]
        public void GetReservationById_ReturnsBadRequest_WhenIdIsNullOrEmpty()
        {
            // Act
            var result1 = _reservationController.GetReservationById(null);
            var result2 = _reservationController.GetReservationById(string.Empty);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result1);
            Assert.IsType<BadRequestObjectResult>(result2);
        }

        [Fact]
        public void GetReservationById_ReturnsNotFound_WhenReservationNotFound()
        {
            // Arrange
            var id = "123";
            _mockReservationsService.Setup(service => service.GetReservationById(id)).Returns((Reservation)null);

            // Act
            var result = _reservationController.GetReservationById(id);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal($"Reservation with ID={id} not found.", notFoundResult.Value);
        }

        [Fact]
        public void GetReservationById_ReturnsOk_WhenReservationIsFound()
        {
            // Arrange
            var id = "123";
            var reservation = new Reservation
            {
                Id = id,
                BookingName = "Test",
                MailId = "test@gmail.com",
                ContactNumber = "12345678",
                Address = "test test test",
                FromDate = DateTime.Now,
                ToDate = DateTime.Now.AddDays(1),
                TotalNumberOfDays = 1,
                RoomNumber = "101",
                RentPerDay = "1000",
                TotalAmount = "1000",
                PaymentStatus = "Pending",
                BookingStatus = "Confirmed"
            };
            _mockReservationsService.Setup(service => service.GetReservationById(id)).Returns(reservation);

            // Act
            var result = _reservationController.GetReservationById(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedReservation = Assert.IsType<Reservation>(okResult.Value);
            Assert.Equal(reservation, returnedReservation);
        }
    }
}
