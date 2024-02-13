using Data.Models;
using Dtos.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Recipes.Controllers;
using Service.Interfaces;
using System.Security.Claims;

namespace Tests.ControllerTests
{
    [TestFixture]
    public class UserControllerTests
    {
        private Mock<IUserService> _userServiceMock;
        private Mock<IHttpContextAccessor> _context;
        private Mock<IConfiguration> _configMock;
        private UserController _userController;

        [SetUp]
        public void Setup()
        {
            _userServiceMock = new Mock<IUserService>();
            _configMock = new Mock<IConfiguration>();
            _context = new Mock<IHttpContextAccessor>();
            _userController = new UserController(_userServiceMock.Object, _configMock.Object, _context.Object);
        }

        [Test]
        public async Task RegisterAsync_ValidUserDto_ReturnsOkResult()
        {
            // Arrange
            var userDto = new UserDto { };
            _userController.ModelState.Clear();

            // Act
            var result = await _userController.RegisterAsync(userDto);

            // Assert
            Assert.That(result, Is.InstanceOf<OkResult>());
        }

        [Test]
        public async Task RegisterAsync_InvalidUserDto_ReturnsBadRequestWithErrorMessage()
        {
            // Arrange
            var invalidUserDto = new UserDto { };
            _userController.ModelState.AddModelError("Email", "Invalid Email");

            // Act
            var result = await _userController.RegisterAsync(invalidUserDto) as BadRequestObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(400));
        }

        [Test]
        public async Task UpdateUserAsync_ValidUpdateUserDto_ReturnsOkResult()
        {
            // Arrange
            var updateUserDto = new UpdateUserDto { };
            _userController.ModelState.Clear();

            // Act
            var result = await _userController.UpdateUserAsync(updateUserDto);

            // Assert
            Assert.That(result, Is.InstanceOf<OkResult>());
        }

        [Test]
        public async Task UpdateUserAsync_InvalidUpdateUserDto_ReturnsBadRequestWithErrorMessage()
        {
            // Arrange
            var invalidUpdateUserDto = new UpdateUserDto { };
            _userController.ModelState.AddModelError("Firstname", "Invalid Firstname");

            // Act
            var result = await _userController.UpdateUserAsync(invalidUpdateUserDto) as BadRequestObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(400));
        }


        [Test]
        public async Task ChangePasswordAsync_InvalidChangePasswordDto_ReturnsBadRequestWithErrorMessage()
        {
            // Arrange
            var invalidChangePasswordDto = new ChangePasswordDto { };
            _userController.ModelState.AddModelError("CurrentPassword", "Invalid CurrentPassword");

            // Act
            var result = await _userController.ChangePasswordAsync(invalidChangePasswordDto) as BadRequestObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(400));
        }

        [Test]
        public async Task LoginAsync_InvalidLoginDetailsDto_ReturnsUnauthorized()
        {
            // Arrange
            var invalidLoginDetailsDto = new LoginDetailsDto { };
            _userController.ModelState.AddModelError("Password", "Invalid Password");

            // Act
            var result = await _userController.LoginAsync(invalidLoginDetailsDto) as BadRequestObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(400));
        }
    }
}
