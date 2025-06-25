namespace AppCloudBlog.Application.Features.Notifications;

public record MarkNotificationAsReadCommand(Guid NotificationId, ClaimsPrincipal User) : IRequest<APIResponse>;

public class MarkNotificationAsReadHandler(INotificationRepository repo)
    : IRequestHandler<MarkNotificationAsReadCommand, APIResponse>
{
    public async Task<APIResponse> Handle(MarkNotificationAsReadCommand request, CancellationToken ct)
    {
        var userId = request.User.GetUserId();
        await repo.MarkAsReadAsync(userId, request.NotificationId);
        return APIResponse.Ok("Marked as read");
    }
}

