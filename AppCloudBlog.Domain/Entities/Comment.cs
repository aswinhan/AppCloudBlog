namespace AppCloudBlog.Domain.Entities;

public class Comment : AuditableBaseEntity
{
    public string Content { get; set; } = string.Empty;
    public string Status { get; set; } = "Pending"; // Approved / Rejected

    public Guid PostId { get; set; }
    public BlogPost Post { get; set; } = default!;

    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    public Guid? ParentId { get; set; }
    public Comment? Parent { get; set; }
    public ICollection<Comment> Replies { get; set; } = [];
}

