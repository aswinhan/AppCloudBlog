namespace AppCloudBlog.Domain.Entities;

public class UserFollow
{
    public Guid FollowerId { get; set; }
    public User Follower { get; set; } = default!;

    public Guid FolloweeId { get; set; }
    public User Followee { get; set; } = default!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}


