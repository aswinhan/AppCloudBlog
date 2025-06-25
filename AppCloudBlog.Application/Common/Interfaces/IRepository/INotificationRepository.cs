namespace AppCloudBlog.Application.Common.Interfaces.IRepository;

public interface INotificationRepository
{
    Task<List<Notification>> GetForUserAsync(Guid userId, bool onlyUnread);
    Task MarkAsReadAsync(Guid userId, Guid notificationId);
    Task MarkAllAsReadAsync(Guid userId);

}
