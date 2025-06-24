namespace AppCloudBlog.Application.Common.Interfaces.IRepository;

public interface IRefreshTokenService
{
    Task<RefreshToken?> GetValidTokenAsync(string token);
    Task RevokeAsync(RefreshToken token);
    Task<RefreshToken> CreateForUserAsync(User user);
    Task RevokeAllAsync(Guid userId);
}
