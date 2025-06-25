namespace AppCloudBlog.Infrastructure.Repository;

public class NotificationRepository(ApplicationDbContext db) : INotificationRepository
{
    private readonly ApplicationDbContext _db = db;

    public async Task<List<Notification>> GetForUserAsync(Guid userId, bool onlyUnread)
    {
        var query = _db.Notifications
            .Where(n => n.UserId == userId);

        if (onlyUnread)
            query = query.Where(n => !n.IsRead);

        return await query
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();
    }


    public async Task MarkAsReadAsync(Guid userId, Guid notificationId)
    {
        var notif = await _db.Notifications
            .FirstOrDefaultAsync(n => n.Id == notificationId && n.UserId == userId);

        if (notif != null && !notif.IsRead)
        {
            notif.IsRead = true;
            await _db.SaveChangesAsync();
        }
    }

    public async Task MarkAllAsReadAsync(Guid userId)
    {
        var unread = await _db.Notifications
            .Where(n => n.UserId == userId && !n.IsRead)
            .ToListAsync();

        if (unread.Count > 0)
        {
            foreach (var n in unread)
                n.IsRead = true;

            await _db.SaveChangesAsync();
        }
    }
}

