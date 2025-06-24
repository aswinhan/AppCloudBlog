namespace AppCloudBlog.Domain.Entities;

public class AdminAuditLog
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid AdminId { get; set; }
    public string Action { get; set; } = default!;
    public string TargetType { get; set; } = default!;
    public string TargetId { get; set; } = default!;
    public string? Details { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
