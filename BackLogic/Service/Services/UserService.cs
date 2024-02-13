using Data.Interfaces;
using Data.Models;
using Dtos.Dtos;
using Microsoft.AspNetCore.Identity;
using Service.Interfaces;

namespace Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;

        public UserService(IUserRepository userRepository, IPasswordHasher<ApplicationUser> passwordHasher, INotificationRepository notificationRepository)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _notificationRepository = notificationRepository;
        }

        public async Task AddUserAsync(UserDto userDto)
        {
            try
            {
                if ((await _userRepository.GetAllUsersAsync()).Any(u => u.Email == userDto.Email))
                {
                    throw new Exception("User with the same email address already exists.");
                }

                var newUser = new ApplicationUser
                {
                    FirstName = userDto.Firstname,
                    LastName = userDto.Lastname,
                    Email = userDto.Email,
                    UserName = userDto.Username
                };

                newUser.PasswordHash = _passwordHasher.HashPassword(newUser, userDto.Password);

                await _userRepository.AddUserAsync(newUser);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateUserAsync(UpdateUserDto userDto)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(userDto.Id);
                if (user == null)
                {
                    throw new Exception("User not found.");
                }

                user.FirstName = userDto.Firstname;
                user.LastName = userDto.Lastname;
                user.Email = userDto.Email;
                user.UserName = userDto.Username;

                await _userRepository.UpdateUserAsync(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> VerifyPasswordAsync(UserDto userDto, string password)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(userDto.Id);
                if (user == null)
                {
                    throw new Exception("User not found.");
                }

                var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
                return passwordVerificationResult == PasswordVerificationResult.Success;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdatePasswordAsync(UserDto userDto, string newPassword)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(userDto.Id);
                if (user == null)
                {
                    throw new Exception("User not found.");
                }

                user.PasswordHash = _passwordHasher.HashPassword(user, newPassword);
                await _userRepository.UpdatePasswordAsync(user, newPassword);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UserDto> GetUserByIdAsync(string id)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(id);
                if (user == null)
                    throw new Exception("User doesn't exist.");

                return new UserDto()
                {
                    Id = id,
                    Email = user.Email,
                    Firstname = user.FirstName,
                    Lastname = user.LastName,
                    Username = user.UserName
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            try
            {
                var users = await _userRepository.GetAllUsersAsync();

                return users.Select(MapUserToDto).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting users.", ex);
            }
        }

        public async Task<List<NotificationDto>> GetAllNotifications(string userID)
        {
            try
            {
                var notifications = await _notificationRepository.GetRecivedNotification(userID);

                return notifications.Select(MapNotificationToDto).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private UserDto MapUserToDto(ApplicationUser user)
        {
            return new UserDto()
            {
                Id = user.Id,
                Email = user.Email,
                Firstname = user.FirstName,
                Lastname = user.LastName,
                Username = user.UserName
            };
        }

        private NotificationDto MapNotificationToDto(Notification notification)
        {
            return new NotificationDto()
            {
                Id = notification.ID,
                SendAt = notification.SentAt,
                SenderId = notification.SenderId,
                ReceiverId = notification.ReceiverId,
                IsRead = notification.IsRead,
                Message = notification.Message,
                NotificationType = notification.NotificationType,
                RecipeId = notification.RecipeId,
                SenderName = notification.Sender?.UserName,
                ReceiverName = notification.Receiver?.UserName,
                RelatedObjectId = notification.RelatedObjectId
            };
        }

        public async Task<ApplicationUser> AuthenticateUserAsync(LoginDetailsDto login)
        {
            try
            {
                var user = await _userRepository.GetUserByEmailAsync(login.Email);
                if (user == null)
                {
                    throw new Exception("User doesn't exist.");
                }

                var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, login.Password);

                if (passwordVerificationResult == PasswordVerificationResult.Success)
                {
                    return user;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task MakeNotificationAsRead(string notificationId)
        {
            try
            {
                await _notificationRepository.MakeAsRead(notificationId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while reading notification.", ex);
            }
        }

        public async Task MakeAllNotificationAsRead(string userId)
        {
            try
            {
                foreach (var not in _notificationRepository.GetRecivedNotification(userId).Result)
                {
                    await _notificationRepository.MakeAsRead(not.ID);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteNotification(string notificationId)
        {
            try
            {
                await _notificationRepository.DeleteNotification(notificationId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
