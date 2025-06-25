namespace AppCloudBlog.Domain.Entities;

public class Notification : AuditableBaseEntity
{
    public Guid UserId { get; set; }  
    public string Title { get; set; } = string.Empty; 
    public string Message { get; set; } = string.Empty;
    public string? Link { get; set; } 
    public bool IsRead { get; set; } = false;

    // Navigation
    public User User { get; set; } = default!;
}

