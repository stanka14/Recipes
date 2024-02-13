using Data.Models;

namespace Data.Interfaces
{
    public interface INotificationRepository : IDisposable
    {
        Task SendNotificationAsync(Notification notification);
        Task MakeAsRead(string notificationId);
        Task DeleteNotification(string notificationId);
        Task<List<Notification>> GetRecivedNotification(string userId);
    }
}
