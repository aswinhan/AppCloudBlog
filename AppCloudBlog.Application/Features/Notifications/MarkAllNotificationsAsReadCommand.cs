namespace AppCloudBlog.Application.Features.Notifications;

public record MarkAllNotificationsAsReadCommand(ClaimsPrincipal User) : IRequest<APIResponse>;

public class MarkAllNotificationsAsReadHandler(INotificationRepository repo)
    : IRequestHandler<MarkAllNotificationsAsReadCommand, APIResponse>
{
    public async Task<APIResponse> Handle(MarkAllNotificationsAsReadCommand request, CancellationToken ct)
    {
        var userId = request.User.GetUserId();
        await repo.MarkAllAsReadAsync(userId);
        return APIResponse.Ok("All notifications marked as read");
    }
}

