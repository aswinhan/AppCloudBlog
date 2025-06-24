namespace AppCloudBlog.Domain.Entities;

public class Comment : AuditableBaseEntity
{
    public string Text { get; set; } = string.Empty;
    public bool IsApproved { get; set; } = false;

    public Guid PostId { get; set; }
    public BlogPost Post { get; set; } = default!;

    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    public Guid? ParentId { get; set; }
    public Comment? Parent { get; set; }
    public ICollection<Comment> Replies { get; set; } = [];
}
