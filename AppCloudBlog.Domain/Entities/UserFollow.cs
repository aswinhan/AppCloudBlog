namespace AppCloudBlog.Domain.Entities;

public class UserFollow
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid FollowerId { get; set; } // the one who follows
    public Guid FolloweeId { get; set; } // the author being followed
    public DateTime FollowedAt { get; set; } = DateTime.UtcNow;

    public User Follower { get; set; } = default!;
    public User Followee { get; set; } = default!;
}

