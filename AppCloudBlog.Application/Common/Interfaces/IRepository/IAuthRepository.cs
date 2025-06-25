namespace AppCloudBlog.Application.Common.Interfaces.IRepository;

public interface IAuthRepository
{
    Task<User?> GetByEmailAsync(string username);
    Task<bool> IsUniqueEmailAsync(string username);
    Task<User> CreateAsync(User user);
    Task<User?> GetByIdAsync(Guid userId);
    Task<User?> GetByUsernameAsync(string username);
    Task<bool> IsFollowingAsync(Guid followerId, Guid followeeId);
    Task FollowAsync(Guid followerId, Guid followeeId);
    Task UnfollowAsync(Guid followerId, Guid followeeId);
    Task<int> GetFollowerCountAsync(Guid userId);
    Task<List<User>> GetFollowersAsync(Guid userId);
    Task<List<User>> GetFollowingAsync(Guid userId);

}

public interface IAuthorizableRequest
{
    string[] Roles { get; }
}

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}

public interface IPasswordHasher
{
    void CreatePasswordHash(string password, out string passwordHash, out string passwordSalt);
    bool VerifyPasswordHash(string password, string storedHash, string storedSalt);
}