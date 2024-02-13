using Data.Enums;
using Data.Interfaces;
using Data.Models;
using Dtos.Dtos;
using Service.Interfaces;
using Message = Data.Models.Message;

namespace Service.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;
        private readonly IUserRepository _userRepository;
        private readonly INotificationRepository _notificationRepository;

        public ChatService(IChatRepository chatRepository, INotificationRepository notificationRepository, IUserRepository userRepository)
        {
            _chatRepository = chatRepository;
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
        }

        public async Task SendMessageAsync(MessageDto message)
        {
            try
            {
                var sender = await _userRepository.GetUserByIdAsync(message.SenderId);
                var chatMessage = new Message() { Text = message.Text, SenderId = message.SenderId, ReceiverId = message.ReceiverId, SentAt = DateTime.Now };
               
                await _chatRepository.SendMessageAsync(chatMessage);

                var notification = new Notification()
                {
                    IsRead = false,
                    SentAt = DateTime.Now,
                    SenderId = message.SenderId,
                    ReceiverId = message.ReceiverId,
                    NotificationType = NotificationType.Message,
                    Recipe = null,
                    Message = $"You have a new message from {sender.UserName}.",
                    RelatedObjectId = chatMessage.ID
                };

                await _notificationRepository.SendNotificationAsync(notification);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while sending a message.", ex);
            }
        }

        public async Task<List<MessageDto>> GetSentMessagesAsync(string userId)
        {
            try
            {
                var messages = await _chatRepository.GetSentMessagesAsync(userId);
                return messages.Select(MapUserToDto).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while retriving sent messages.", ex);
            }
        }

        public async Task<List<MessageDto>> GetRecivedMessagesAsync(string userId)
        {
            try
            {
                var messages = await _chatRepository.GetRecivedMessagesAsync(userId);
                return messages.Select(MapUserToDto).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while retriving recived messages.", ex);
            }
        }

        public async Task<List<MessageDto>> GetMessagesBetweenUsersAsync(string loggedInUser, string withUserId)
        {
            try
            {
                var messages = await _chatRepository.GetMessagesBetweenUsersAsync(loggedInUser, withUserId);

                var notifications = await _notificationRepository.GetRecivedNotification(loggedInUser);

                foreach (var notification in notifications)
                {
                    if (messages.Any(m => m.ID == notification.RelatedObjectId))
                    {
                        await _notificationRepository.MakeAsRead(notification.ID);
                    }
                }

                return messages.Select(MapUserToDto).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while retriving recived messages.", ex);
            }
        }

        private MessageDto MapUserToDto(Message message)
        {
            return new MessageDto()
            {
                Id = message.ID,
                SenderId = message.SenderId,
                ReceiverId = message.ReceiverId,
                Text = message.Text,
                SendAt = message.SentAt,
                SenderName = message.Sender?.UserName,
                ReceiverName = message.Receiver?.UserName
            };
        }
    }
}
