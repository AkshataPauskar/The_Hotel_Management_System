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
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly UserController _userController;

        public UserControllerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _userController = new UserController(_mockUserService.Object);
        }

        [Fact]
        public void Login_ReturnsBadRequest_WhenLoginDetailsAreNull()
        {
            // Act
            var result = _userController.Login(null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Login_ReturnsUnauthorized_WhenUserIsInvalid()
        {
            // Arrange
            var loginDetails = new Login { UserName = "test", Password = "test" };
            _mockUserService.Setup(service => service.ValidateUser(loginDetails)).Returns((User)null);

            // Act
            var result = _userController.Login(loginDetails);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Invalid username or password.", unauthorizedResult.Value);
        }

        [Fact]
        public void Login_ReturnsOk_WhenUserIsValid()
        {
            // Arrange
            var loginDetails = new Login { UserName = "test", Password = "test" };
            var user = new User { 
                EmailId="abc@gmail.com", 
                UserName = "test", 
                Password="test", 
                IsAdmin=true,
                CreatedOn=DateTime.Now };

            _mockUserService.Setup(service => service.ValidateUser(loginDetails)).Returns(user);

            // Act
            var result = _userController.Login(loginDetails);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Login successful", okResult.Value);
        }
    }
}
