namespace AppCloudBlog.Domain.Entities;

public class PostTag
{
    public Guid PostId { get; set; }
    public BlogPost Post { get; set; } = default!;

    public Guid TagId { get; set; }
    public Tag Tag { get; set; } = default!;
}
