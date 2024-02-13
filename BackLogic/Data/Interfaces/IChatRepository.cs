using Data.Models;

namespace Data.Interfaces
{
    public interface IChatRepository : IDisposable
    {
        Task SendMessageAsync(Message message);
        Task<List<Message>> GetRecivedMessagesAsync(string userId);
        Task<List<Message>> GetSentMessagesAsync(string userId);
        Task<List<Message>> GetMessagesBetweenUsersAsync(string userId1, string userId2);
    }
}
