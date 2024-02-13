using Data.Interfaces;
using Data.Models;
using Dtos.Dtos;
using Microsoft.AspNetCore.Identity;
using Moq;
using Service.Services;

namespace Tests.ServiceTests
{
    [TestFixture]
    public class UserServiceTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<INotificationRepository> _notificationRepositoryMock;
        private Mock<IPasswordHasher<ApplicationUser>> _passwordHasherMock;
        private UserService _userService;

        [SetUp]
        public void Setup()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _notificationRepositoryMock = new Mock<INotificationRepository>();
            _passwordHasherMock = new Mock<IPasswordHasher<ApplicationUser>>();
            _userService = new UserService(_userRepositoryMock.Object, _passwordHasherMock.Object, _notificationRepositoryMock.Object);
        }

        [Test]
        public void AddUserAsync_UserWithEmailExists_ThrowsException()
        {
            // Arrange
            var existingUser = new ApplicationUser { Email = "john.doe@example.com" };
            var userDto = new UserDto { Email = "john.doe@example.com" };
            _userRepositoryMock.Setup(repo => repo.GetAllUsersAsync()).ReturnsAsync(new List<ApplicationUser> { existingUser });

            // Act & Assert
            Assert.ThrowsAsync<Exception>(async () => await _userService.AddUserAsync(userDto));
        }

        [Test]
        public async Task UpdateUserAsync_ValidData_ReturnsTask()
        {
            // Arrange
            var updateUserDto = new UpdateUserDto { Id = "userId", Firstname = "John", Lastname = "Doe", Email = "john.doe@example.com", Username = "johndoe" };
            var existingUser = new ApplicationUser { Id = "userId" };
            _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(updateUserDto.Id)).ReturnsAsync(existingUser);
            _userRepositoryMock.Setup(repo => repo.UpdateUserAsync(It.IsAny<ApplicationUser>()));

            // Act
            await _userService.UpdateUserAsync(updateUserDto);

            // Assert
        }

        [Test]
        public void UpdateUserAsync_UserNotFound_ThrowsException()
        {
            // Arrange
            var updateUserDto = new UpdateUserDto { Id = "userId" };
            _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(updateUserDto.Id)).ReturnsAsync((ApplicationUser)null);

            // Act & Assert
            Assert.ThrowsAsync<Exception>(async () => await _userService.UpdateUserAsync(updateUserDto));
        }

        [Test]
        public async Task VerifyPasswordAsync_ValidPassword_ReturnsTrue()
        {
            // Arrange
            var userDto = new UserDto { Id = "userId" };
            var user = new ApplicationUser { Id = "userId", PasswordHash = "hashed_password" };
            _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userDto.Id)).ReturnsAsync(user);
            _passwordHasherMock.Setup(ph => ph.VerifyHashedPassword(user, user.PasswordHash, "password")).Returns(PasswordVerificationResult.Success);

            // Act
            var result = await _userService.VerifyPasswordAsync(userDto, "password");

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task VerifyPasswordAsync_InvalidPassword_ReturnsFalse()
        {
            // Arrange
            var userDto = new UserDto { Id = "userId" };
            var user = new ApplicationUser { Id = "userId", PasswordHash = "hashed_password" };
            _userRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userDto.Id)).ReturnsAsync(user);
            _passwordHasherMock.Setup(ph => ph.VerifyHashedPassword(user, user.PasswordHash, "wrong_password")).Returns(PasswordVerificationResult.Failed);

            // Act
            var result = await _userService.VerifyPasswordAsync(userDto, "wrong_password");

            // Assert
            Assert.IsFalse(result);
        }
    }
}
