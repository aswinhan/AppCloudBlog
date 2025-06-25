namespace AppCloudBlog.Infrastructure.Repository;

public class TagRepository(ApplicationDbContext db) : ITagRepository
{
    private readonly ApplicationDbContext _db = db;

    public async Task<List<Tag>> ResolveTagsAsync(IEnumerable<string> tagNames)
    {
        var distinctNames = tagNames.Select(t => t.Trim().ToLower()).Distinct().ToList();

        // Fetch existing tags from DB
        var existingTags = await _db.Tags
            .Where(t => distinctNames.Contains(t.Name.ToLower()))
            .ToListAsync();

        // Determine new tags to add
        var newTagNames = distinctNames
            .Except(existingTags.Select(t => t.Name.ToLower()))
            .ToList();

        var newTags = newTagNames
            .Select(name => new Tag { Name = name })
            .ToList();

        _db.Tags.AddRange(newTags);
        await _db.SaveChangesAsync();

        return existingTags.Concat(newTags).ToList();
    }

    public async Task<List<Tag>> GetAllAsync()
    {
        return await _db.Tags
            .OrderBy(t => t.Name)
            .ToListAsync();
    }
}
