namespace AppCloudBlog.Application.Features.Tags;

public record GetAllTagsQuery() : IRequest<APIResponse>;

public class GetAllTagsQueryHandler(ITagRepository tagRepo)
    : IRequestHandler<GetAllTagsQuery, APIResponse>
{
    public async Task<APIResponse> Handle(GetAllTagsQuery request, CancellationToken ct)
    {
        var tags = await tagRepo.GetAllAsync();

        var tagNames = tags.Select(t => t.Name).ToList();

        return APIResponse.Ok(tagNames);
    }
}
