namespace AppCloudBlog.Application.Common.Interfaces.IRepository;

public interface ICommentRepository
{
    Task<Comment> AddAsync(Comment comment);
    Task<List<Comment>> GetByPostIdAsync(Guid postId);
    Task<List<Comment>> GetByUserIdAsync(Guid userId);
    Task<Comment?> GetByIdAsync(Guid id);
    Task UpdateAsync(Comment comment);
}
