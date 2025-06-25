namespace AppCloudBlog.Application.DTOs.Comment;

public record CommentDto(
    Guid Id,
    string Content,
    string Status,
    DateTime CreatedAt,
    string AuthorName,
    Guid? ParentId,
    List<CommentDto>? Replies = null
);
