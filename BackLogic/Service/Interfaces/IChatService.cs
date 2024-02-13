using Dtos.Dtos;

namespace Service.Interfaces
{
    public interface IChatService
    {
        Task SendMessageAsync(MessageDto message);
        Task<List<MessageDto>> GetRecivedMessagesAsync(string userId);
        Task<List<MessageDto>> GetSentMessagesAsync(string userId);
        Task<List<MessageDto>> GetMessagesBetweenUsersAsync(string userId1, string userId2);
    }
}
