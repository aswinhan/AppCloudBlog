namespace AppCloudBlog.Api.Endpoints;

public static class NotificationEndpoints
{
    public static void MapNotificationEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/notifications", async (
            HttpContext ctx,
            [FromQuery] bool unreadOnly,
            ISender sender) =>
        {
            var result = await sender.Send(new GetMyNotificationsQuery(ctx.User, unreadOnly));
            return Results.Json(result, statusCode: (int)result.StatusCode);
        })
        .WithName("GetMyNotifications")
        .WithTags("Notifications")
        .Produces<APIResponse>(200)
        .RequireAuthorization();

        // Mark single notification as read
        group.MapPut("/notifications/{id}/read", async (
            HttpContext ctx,
            Guid id,
            ISender sender) =>
        {
            var result = await sender.Send(new MarkNotificationAsReadCommand(id, ctx.User));
            return Results.Json(result, statusCode: (int)result.StatusCode);
        })
        .WithName("MarkNotificationAsRead")
        .WithTags("Notifications")
        .Produces<APIResponse>(200)
        .RequireAuthorization();

        // Mark all notifications as read
        group.MapPut("/notifications/read-all", async (
            HttpContext ctx,
            ISender sender) =>
        {
            var result = await sender.Send(new MarkAllNotificationsAsReadCommand(ctx.User));
            return Results.Json(result, statusCode: (int)result.StatusCode);
        })
        .WithName("MarkAllNotificationsAsRead")
        .WithTags("Notifications")
        .Produces<APIResponse>(200)
        .RequireAuthorization();

    }
}

