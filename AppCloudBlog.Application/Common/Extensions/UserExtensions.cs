namespace AppCloudBlog.Application.Common.Extensions;

public static class UserExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            throw new UnauthorizedAccessException("User ID claim is missing or invalid.");
        }

        return userId;
    }

    public static string GetUsername(this ClaimsPrincipal user)
    {
        return user.FindFirst(ClaimTypes.Name)?.Value
            ?? throw new UnauthorizedAccessException("Username claim is missing.");
    }

    public static string GetUserRole(this ClaimsPrincipal user)
    {
        return user.FindFirst(ClaimTypes.Role)?.Value
            ?? throw new UnauthorizedAccessException("Role claim is missing.");
    }

    public static bool IsInRole(this ClaimsPrincipal user, string role)
    {
        return user.IsInRole(role);
    }
}
