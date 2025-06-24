namespace AppCloudBlog.Domain.Entities;

public class Notification
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid RecipientId { get; set; }
    public string Type { get; set; } = default!; // e.g., "Comment", "Like"
    public string Message { get; set; } = default!;
    public string? Link { get; set; } // e.g., "/posts/slug"
    public bool IsRead { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User Recipient { get; set; } = default!;
}
