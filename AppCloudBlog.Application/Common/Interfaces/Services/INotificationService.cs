namespace AppCloudBlog.Application.Common.Interfaces.Services;

public interface INotificationService
{
    Task NotifyFollowersOnNewPostAsync(User author, BlogPost post);
}
