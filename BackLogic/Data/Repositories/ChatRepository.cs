using Data.DbContext;
using Data.Interfaces;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class ChatRepository : IChatRepository, IDisposable
    {
        private readonly ApplicationDbContext _dbContext;

        public ChatRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<List<Message>> GetRecivedMessagesAsync(string userId)
        {
            try
            {
                return await _dbContext.Messages
                    .Where(msg => msg.ReceiverId == userId)
                    .OrderBy(m => m.SentAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while retrieving received messages.", ex);
            }
        }

        public async Task<List<Message>> GetMessagesBetweenUsersAsync(string userId1, string userId2)
        {
            try
            {
                var messages = await _dbContext.Messages
                   .Where(m => (m.SenderId == userId1 && m.ReceiverId == userId2) || (m.SenderId == userId2 && m.ReceiverId == userId1))
                   .OrderBy(m => m.SentAt)
                   .ToListAsync();

                return messages;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while retrieving messages between users.", ex);
            }
        }

        public async Task<List<Message>> GetSentMessagesAsync(string userId)
        {
            try
            {
                return await _dbContext.Messages
                    .Where(msg => msg.SenderId == userId)
                    .OrderBy(m => m.SentAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while retrieving sent messages.", ex);
            }
        }

        public async Task SendMessageAsync(Message message)
        {
            try
            {
                _dbContext.Messages.Add(message);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while sending a message.", ex);
            }
        }

        public void Dispose() => _dbContext.Dispose();
    }
}
