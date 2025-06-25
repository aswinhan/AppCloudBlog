namespace AppCloudBlog.Domain.Entities;

public class Tag : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public ICollection<PostTag> PostTags { get; set; } = [];
}

