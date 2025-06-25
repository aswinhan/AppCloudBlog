namespace AppCloudBlog.Infrastructure.Repository;

public class BlogPostRepository(ApplicationDbContext db) : IBlogPostRepository
{
    private readonly ApplicationDbContext _db = db;

    public async Task AddAsync(BlogPost post)
    {
        _db.BlogPosts.Add(post);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(BlogPost post)
    {
        _db.BlogPosts.Update(post);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(BlogPost post)
    {
        post.IsDeleted = true;
        await _db.SaveChangesAsync();
    }

    public async Task<BlogPost?> GetByIdAsync(Guid id)
        => await _db.BlogPosts.Include(p => p.PostTags).ThenInclude(t => t.Tag)
                              .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

    public async Task<BlogPost?> GetBySlugAsync(string slug)
        => await _db.BlogPosts.Include(p => p.PostTags).ThenInclude(t => t.Tag)
                              .FirstOrDefaultAsync(p => p.Slug == slug && !p.IsDeleted);

    public async Task<List<BlogPost>> GetAllPublicAsync()
        => await _db.BlogPosts
            .Include(p => p.PostTags).ThenInclude(t => t.Tag)
            .Where(p => p.Status == "Published" && !p.IsDeleted)
            .OrderByDescending(p => p.PublishAt ?? p.CreatedAt)
            .ToListAsync();

    public async Task<List<BlogPost>> GetByAuthorAsync(Guid authorId)
        => await _db.BlogPosts
            .Include(p => p.PostTags).ThenInclude(t => t.Tag)
            .Where(p => p.AuthorId == authorId && !p.IsDeleted)
            .ToListAsync();

    public async Task<bool> IsSlugUniqueAsync(string slug)
        => !await _db.BlogPosts.AnyAsync(p => p.Slug == slug && !p.IsDeleted);

    public async Task<(List<BlogPost> Posts, int TotalCount)> GetPublicPagedAsync(int page, int pageSize)
    {
        var query = _db.BlogPosts
            .Include(p => p.PostTags).ThenInclude(t => t.Tag)
            .Where(p => p.Status == "Published" && !p.IsDeleted)
            .OrderByDescending(p => p.PublishAt ?? p.CreatedAt);

        var total = await query.CountAsync();

        var posts = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (posts, total);
    }

    public async Task<(List<BlogPost> Posts, int TotalCount)> GetAuthorPagedAsync(Guid authorId, int page, int pageSize, string? status = null)
    {
        var query = _db.BlogPosts
            .Include(p => p.PostTags).ThenInclude(t => t.Tag)
            .Where(p => p.AuthorId == authorId && !p.IsDeleted);

        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(p => p.Status == status);
        }

        var total = await query.CountAsync();

        var posts = await query
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (posts, total);
    }
    public async Task<(List<BlogPost> Posts, int TotalCount)> SearchPublicPostsAsync(string? keyword, List<string>? tags, int page, int pageSize)
    {
        var query = _db.BlogPosts
            .Include(p => p.PostTags).ThenInclude(pt => pt.Tag)
            .Where(p => p.Status == "Published" && !p.IsDeleted);

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(p =>
                p.Title.Contains(keyword) || p.Content.Contains(keyword));
        }

        if (tags != null && tags.Count > 0)
        {
            query = query.Where(p =>
                p.PostTags.Any(pt => tags.Contains(pt.Tag.Name)));
        }

        var total = await query.CountAsync();

        var posts = await query
            .OrderByDescending(p => p.PublishAt ?? p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (posts, total);
    }

    public async Task<bool> HasUserLikedAsync(Guid postId, Guid userId)
    {
        return await _db.PostLikes.AnyAsync(pl => pl.PostId == postId && pl.UserId == userId);
    }

    public async Task AddLikeAsync(Guid postId, Guid userId)
    {
        _db.PostLikes.Add(new PostLike { PostId = postId, UserId = userId });
        await _db.SaveChangesAsync();
    }

    public async Task RemoveLikeAsync(Guid postId, Guid userId)
    {
        var like = await _db.PostLikes
            .FirstOrDefaultAsync(pl => pl.PostId == postId && pl.UserId == userId);
        if (like is not null)
        {
            _db.PostLikes.Remove(like);
            await _db.SaveChangesAsync();
        }
    }

    public async Task<int> GetLikeCountAsync(Guid postId)
    {
        return await _db.PostLikes.CountAsync(pl => pl.PostId == postId);
    }

    public async Task<bool> IsBookmarkedAsync(Guid postId, Guid userId)
    {
        return await _db.PostBookmarks.AnyAsync(b => b.PostId == postId && b.UserId == userId);
    }

    public async Task AddBookmarkAsync(Guid postId, Guid userId)
    {
        _db.PostBookmarks.Add(new PostBookmark { PostId = postId, UserId = userId });
        await _db.SaveChangesAsync();
    }

    public async Task RemoveBookmarkAsync(Guid postId, Guid userId)
    {
        var bookmark = await _db.PostBookmarks.FirstOrDefaultAsync(b => b.PostId == postId && b.UserId == userId);
        if (bookmark is not null)
        {
            _db.PostBookmarks.Remove(bookmark);
            await _db.SaveChangesAsync();
        }
    }

    public async Task<List<BlogPost>> GetBookmarkedPostsAsync(Guid userId)
    {
        return await _db.PostBookmarks
            .Where(b => b.UserId == userId)
            .Select(b => b.Post)
            .Include(p => p.PostTags).ThenInclude(t => t.Tag)
            .Where(p => p.Status == "Published" && !p.IsDeleted)
            .OrderByDescending(p => p.PublishAt ?? p.CreatedAt)
            .ToListAsync();
    }



}
