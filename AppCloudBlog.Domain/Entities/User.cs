namespace AppCloudBlog.Domain.Entities;

public class User : AuditableBaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Username { get; set; } = default!;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string PasswordSalt { get; set; } = string.Empty;
    public string Role { get; set; } = "Reader"; // Admin, Author, Reader
    public string? AvatarUrl { get; set; }
    

    // Navigation
    public ICollection<BlogPost> BlogPosts { get; set; } = [];
    public ICollection<Comment> Comments { get; set; } = [];
    public ICollection<RefreshToken> RefreshTokens { get; set; } = [];

}
