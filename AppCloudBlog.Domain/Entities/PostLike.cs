namespace AppCloudBlog.Domain.Entities;

public class PostLike
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public Guid PostId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; } = default!;
    public BlogPost Post { get; set; } = default!;
}
