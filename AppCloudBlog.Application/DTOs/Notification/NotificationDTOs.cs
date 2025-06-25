namespace AppCloudBlog.Application.DTOs.Notification;

public record NotificationDto(
    Guid Id,
    string Title,
    string Message,
    string? Link,
    DateTime CreatedAt,
    bool IsRead
);
