namespace AppCloudBlog.Infrastructure.Repository;

public class AuthRepository(ApplicationDbContext db) : IAuthRepository
{
    private readonly ApplicationDbContext _db = db;

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _db.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());
    }

    public async Task<bool> IsUniqueEmailAsync(string email)
    {
        return !await _db.Users.AnyAsync(x => x.Email == email);
    }

    public async Task<User> CreateAsync(User user)
    {
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return user;
    }

    public async Task<User?> GetByIdAsync(Guid userId)
    {
        return await _db.Users.FindAsync(userId);
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _db.Users
            .Include(u => u.BlogPosts)
            .FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
    }

    public async Task<bool> IsFollowingAsync(Guid followerId, Guid followeeId)
    {
        return await _db.UserFollows.AnyAsync(f => f.FollowerId == followerId && f.FolloweeId == followeeId);
    }

    public async Task FollowAsync(Guid followerId, Guid followeeId)
    {
        _db.UserFollows.Add(new UserFollow { FollowerId = followerId, FolloweeId = followeeId });
        await _db.SaveChangesAsync();
    }

    public async Task UnfollowAsync(Guid followerId, Guid followeeId)
    {
        var rel = await _db.UserFollows.FirstOrDefaultAsync(f => f.FollowerId == followerId && f.FolloweeId == followeeId);
        if (rel != null)
        {
            _db.UserFollows.Remove(rel);
            await _db.SaveChangesAsync();
        }
    }

    public async Task<int> GetFollowerCountAsync(Guid userId)
    {
        return await _db.UserFollows.CountAsync(f => f.FolloweeId == userId);
    }

    public async Task<List<User>> GetFollowersAsync(Guid userId)
    {
        return await _db.UserFollows
            .Where(f => f.FolloweeId == userId)
            .Select(f => f.Follower)
            .ToListAsync();
    }

    public async Task<List<User>> GetFollowingAsync(Guid userId)
    {
        return await _db.UserFollows
            .Where(f => f.FollowerId == userId)
            .Select(f => f.Followee)
            .ToListAsync();
    }

}

