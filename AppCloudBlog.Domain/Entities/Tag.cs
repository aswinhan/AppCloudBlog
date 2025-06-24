namespace AppCloudBlog.Domain.Entities;

public class Tag : AuditableBaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;

    public ICollection<PostTag> PostTags { get; set; } = [];
}
