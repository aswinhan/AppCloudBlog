namespace AppCloudBlog.Domain.Entities;

public class PostBookmark : BaseEntity
{
    public Guid PostId { get; set; }
    public BlogPost Post { get; set; } = default!;

    public Guid UserId { get; set; }
    public User User { get; set; } = default!;
}


