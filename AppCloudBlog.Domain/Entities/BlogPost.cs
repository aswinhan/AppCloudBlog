namespace AppCloudBlog.Domain.Entities;

public class BlogPost : AuditableBaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = default!;
    public string Content { get; set; } = string.Empty;
    public string? CoverImageUrl { get; set; }
    public string Status { get; set; } = "Draft"; // Published/Draft
    public DateTime? PublishAt { get; set; }

    public Guid AuthorId { get; set; }
    public User Author { get; set; } = default!;

    public ICollection<Comment> Comments { get; set; } = [];
    public ICollection<PostTag> PostTags { get; set; } = [];
    public ICollection<PostLike> PostLikes { get; set; } = [];
    public ICollection<PostBookmark> PostBookmarks { get; set; } = [];
}

