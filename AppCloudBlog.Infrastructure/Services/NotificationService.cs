using AppCloudBlog.Application.Common.Interfaces.Services;

namespace AppCloudBlog.Infrastructure.Services;

public class NotificationService(
    ApplicationDbContext db) : INotificationService
{
    public async Task NotifyFollowersOnNewPostAsync(User author, BlogPost post)
    {
        var followerIds = await db.UserFollows
            .Where(f => f.FolloweeId == author.Id)
            .Select(f => f.FollowerId)
            .ToListAsync();

        if (!followerIds.Any()) return;

        var notifications = followerIds.Select(fid => new Notification
        {
            UserId = fid,
            Title = $"New post by {author.Name}",
            Message = post.Title,
            Link = $"/posts/{post.Slug}",
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        });

        db.Notifications.AddRange(notifications);
        await db.SaveChangesAsync();
    }
}

