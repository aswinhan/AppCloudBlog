namespace AppCloudBlog.Application.DTOs.Blog;

public record BlogPostDto(
    Guid Id,
    string Title,
    string Slug,
    string Content,
    string? CoverImageUrl,
    string Status,
    DateTime CreatedAt,
    DateTime? PublishAt,
    List<string> Tags,
    bool LikedByMe = false,
    int LikeCount = 0,
    bool BookmarkedByMe = false
);


public record CreateBlogPostRequestDto(
    string Title,
    string Content,
    List<string> Tags,
    string? CoverImageUrl = null,
    DateTime? PublishAt = null
);

public record UpdateBlogPostRequestDto(
    string Title,
    string Content,
    List<string> Tags,
    string? CoverImageUrl = null,
    DateTime? PublishAt = null
);
