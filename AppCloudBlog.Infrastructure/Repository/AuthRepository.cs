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

}

