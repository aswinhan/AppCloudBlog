namespace AppCloudBlog.Application.DTOs.User;

public record AuthorProfileDto(
    Guid Id,
    string Username,
    string Name,
    string? AvatarUrl,
    int TotalPosts,
    bool IsFollowedByMe, // set to false for now if not implementing
    int FollowerCount
);

public record AuthorSummaryDto(
    Guid Id,
    string Username,
    string Name,
    string? AvatarUrl
);