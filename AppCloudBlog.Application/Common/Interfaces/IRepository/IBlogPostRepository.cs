namespace AppCloudBlog.Application.Common.Interfaces.IRepository;

public interface IBlogPostRepository
{
    Task<BlogPost?> GetByIdAsync(Guid id);
    Task<BlogPost?> GetBySlugAsync(string slug);
    Task<List<BlogPost>> GetAllPublicAsync();
    Task<List<BlogPost>> GetByAuthorAsync(Guid authorId);
    Task AddAsync(BlogPost post);
    Task UpdateAsync(BlogPost post);
    Task DeleteAsync(BlogPost post);
    Task<bool> IsSlugUniqueAsync(string slug);
    Task<(List<BlogPost> Posts, int TotalCount)> GetPublicPagedAsync(int page, int pageSize);
    Task<(List<BlogPost> Posts, int TotalCount)> GetAuthorPagedAsync(Guid authorId, int page, int pageSize, string? status = null);
    Task<(List<BlogPost> Posts, int TotalCount)> SearchPublicPostsAsync(string? keyword, List<string>? tags, int page, int pageSize);
    Task<bool> HasUserLikedAsync(Guid postId, Guid userId);
    Task AddLikeAsync(Guid postId, Guid userId);
    Task RemoveLikeAsync(Guid postId, Guid userId);
    Task<int> GetLikeCountAsync(Guid postId);

    Task<bool> IsBookmarkedAsync(Guid postId, Guid userId);
    Task AddBookmarkAsync(Guid postId, Guid userId);
    Task RemoveBookmarkAsync(Guid postId, Guid userId);
    Task<List<BlogPost>> GetBookmarkedPostsAsync(Guid userId);

}
