namespace AppCloudBlog.Domain.Entities;

public class Comment : AuditableBaseEntity
{
    public string Content { get; set; } = string.Empty;
    public Guid PostId { get; set; }
    public Guid UserId { get; set; }
    public Guid? ParentId { get; set; } // for nested replies
    public string Status { get; set; } = "Pending"; // Approved, Rejected, Pending

    // Navigation
    public BlogPost Post { get; set; } = default!;
    public User User { get; set; } = default!;
    public Comment? Parent { get; set; }
    public ICollection<Comment> Replies { get; set; } = [];
}
