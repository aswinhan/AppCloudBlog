namespace AppCloudBlog.Infrastructure.Options;

public class JwtSettings
{
    public string Secret { get; set; } = string.Empty;
    public int ExpiryDays { get; set; } = 7;
    public string Issuer { get; set; } = "AppCloudBlog";
    public string Audience { get; set; } = "AppCloudBlogUsers";
}
