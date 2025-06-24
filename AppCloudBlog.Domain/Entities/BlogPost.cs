namespace AppCloudBlog.Domain.Entities;

public class BlogPost : AuditableBaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public bool IsPublished { get; set; } = false;
    public DateTime? PublishedAt { get; set; }
    public DateTime? PublishAt { get; set; }
    public Guid AuthorId { get; set; }
    public User Author { get; set; } = default!;
    public int Views { get; set; } = 0;


    // Relationships
    public ICollection<Comment> Comments { get; set; } = [];
    public ICollection<PostTag> PostTags { get; set; } = [];
    public Guid CategoryId { get; set; }
    public Category Category { get; set; } = default!;
    public ICollection<PostLike> Likes { get; set; } = [];

}
