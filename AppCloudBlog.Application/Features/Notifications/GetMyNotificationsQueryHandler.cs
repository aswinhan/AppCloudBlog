using AppCloudBlog.Application.DTOs.Notification;

namespace AppCloudBlog.Application.Features.Notifications;

public record GetMyNotificationsQuery(ClaimsPrincipal User, bool OnlyUnread) : IRequest<APIResponse>;

public class GetMyNotificationsQueryHandler(INotificationRepository repo)
    : IRequestHandler<GetMyNotificationsQuery, APIResponse>
{
    public async Task<APIResponse> Handle(GetMyNotificationsQuery request, CancellationToken ct)
    {
        var userId = request.User.GetUserId();
        var list = await repo.GetForUserAsync(userId, request.OnlyUnread);

        var dto = list.Select(n => new NotificationDto(
            n.Id, n.Title, n.Message, n.Link, n.CreatedAt, n.IsRead
        )).ToList();

        return APIResponse.Ok(dto);
    }
}
