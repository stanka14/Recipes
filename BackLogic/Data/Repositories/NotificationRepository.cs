using Data.DbContext;
using Data.Interfaces;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class NotificationRepository : INotificationRepository, IDisposable
    {
        private readonly ApplicationDbContext _dbContext;
        public NotificationRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task SendNotificationAsync(Notification notification)
        {
            try
            {
                await _dbContext.Notifications.AddAsync(notification);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while sending a notification.", ex);
            }
        }

        public async Task<List<Notification>> GetRecivedNotification(string userId)
        {
            try
            {
                return await _dbContext.Notifications
                    .Where(msg => msg.ReceiverId == userId && !msg.IsRead)
                    .Include(m => m.Sender)
                    .Include(m => m.Receiver)
                    .OrderByDescending(m => m.SentAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while retriving recieved notifications.", ex);
            }
        }

        public async Task MakeAsRead(string notificationId)
        {
            try
            {
                var notification = _dbContext.Notifications.FirstOrDefault(x => x.ID == notificationId);

                if (notification == null)
                {
                    throw new Exception("Notification not found.");
                }

                notification.IsRead = true;
                await _dbContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new Exception("Error while sending a notification.", ex);
            }
        }
        public async Task DeleteNotification(string notificationId)
        {
            try
            {
                var notification = await _dbContext.Notifications.FindAsync(notificationId);
                if (notification == null)
                {
                    throw new Exception("Notification not found.");
                }

                _dbContext.Notifications.Remove(notification);
                await _dbContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new Exception("Error while deleting a notification.", ex);
            }
        }
        public void Dispose() => _dbContext.Dispose();
    }
}