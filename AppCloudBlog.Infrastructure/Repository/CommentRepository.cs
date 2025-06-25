namespace AppCloudBlog.Infrastructure.Repository;

public class CommentRepository(ApplicationDbContext db) : ICommentRepository
{
    private readonly ApplicationDbContext _db = db;

    public async Task<Comment> AddAsync(Comment comment)
    {
        _db.Comments.Add(comment);
        await _db.SaveChangesAsync();
        return comment;
    }

    public async Task<List<Comment>> GetByPostIdAsync(Guid postId)
    {
        return await _db.Comments
            .Include(c => c.Replies)
            .Include(c => c.User)
            .Where(c => c.PostId == postId && c.ParentId == null && !c.IsDeleted)
            .OrderBy(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Comment>> GetByUserIdAsync(Guid userId)
    {
        return await _db.Comments
            .Include(c => c.Post)
            .Where(c => c.UserId == userId && !c.IsDeleted)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<Comment?> GetByIdAsync(Guid id)
    {
        return await _db.Comments
            .Include(c => c.Replies)
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
    }

    public async Task UpdateAsync(Comment comment)
    {
        _db.Comments.Update(comment);
        await _db.SaveChangesAsync();
    }
}
