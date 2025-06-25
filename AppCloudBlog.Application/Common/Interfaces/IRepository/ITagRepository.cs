namespace AppCloudBlog.Application.Common.Interfaces.IRepository;

public interface ITagRepository
{
    Task<List<Tag>> ResolveTagsAsync(IEnumerable<string> tagNames);
    Task<List<Tag>> GetAllAsync();
}
