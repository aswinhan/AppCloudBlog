namespace AppCloudBlog.Domain.Entities;

public class User : AuditableBaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Username { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = string.Empty;
    public string PasswordSalt { get; set; } = string.Empty;
    public string Role { get; set; } = "Reader";
    public string? AvatarUrl { get; set; }

    public ICollection<BlogPost> BlogPosts { get; set; } = [];
    public ICollection<Comment> Comments { get; set; } = [];
    public ICollection<PostLike> PostLikes { get; set; } = [];
    public ICollection<PostBookmark> PostBookmarks { get; set; } = [];
    public ICollection<UserFollow> Followers { get; set; } = [];
    public ICollection<UserFollow> Following { get; set; } = [];
    public ICollection<Notification> Notifications { get; set; } = [];
    public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
}

