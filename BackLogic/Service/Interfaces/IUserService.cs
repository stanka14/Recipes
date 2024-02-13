using Data.Models;
using Dtos.Dtos;

namespace Service.Interfaces
{
    public interface IUserService
    {
        Task AddUserAsync(UserDto userDto);
        Task<UserDto> GetUserByIdAsync(string id);
        Task<List<UserDto>> GetAllUsersAsync();
        Task<List<NotificationDto>> GetAllNotifications(string userID);
        Task MakeNotificationAsRead(string notificationId);
        Task DeleteNotification(string notificationId);
        Task MakeAllNotificationAsRead(string userId);
        Task UpdateUserAsync(UpdateUserDto userDto);
        Task<bool> VerifyPasswordAsync(UserDto userDto, string password);
        Task UpdatePasswordAsync(UserDto userDto, string password);
        Task<ApplicationUser> AuthenticateUserAsync(LoginDetailsDto login);
    }
}
