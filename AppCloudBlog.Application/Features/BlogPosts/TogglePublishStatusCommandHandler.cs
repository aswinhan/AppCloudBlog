namespace AppCloudBlog.Application.Features.BlogPosts;

public record TogglePublishStatusCommand(string SlugOrId, ClaimsPrincipal User) : IRequest<APIResponse>;

public class TogglePublishStatusCommandHandler(IBlogPostRepository blogRepo)
    : IRequestHandler<TogglePublishStatusCommand, APIResponse>
{
    public async Task<APIResponse> Handle(TogglePublishStatusCommand request, CancellationToken ct)
    {
        BlogPost? post;

        if (Guid.TryParse(request.SlugOrId, out var id))
            post = await blogRepo.GetByIdAsync(id);
        else
            post = await blogRepo.GetBySlugAsync(request.SlugOrId);

        if (post is null || post.IsDeleted)
            return APIResponse.Fail("Post not found", HttpStatusCode.NotFound);

        var userId = request.User.GetUserId();
        if (post.AuthorId != userId)
            return APIResponse.Fail("Unauthorized", HttpStatusCode.Forbidden);

        if (post.Status == "Published")
        {
            post.Status = "Draft";
        }
        else
        {
            post.Status = "Published";
            post.PublishAt ??= DateTime.UtcNow;
        }

        post.UpdatedAt = DateTime.UtcNow;

        await blogRepo.UpdateAsync(post);

        return APIResponse.Ok($"Post status updated to '{post.Status}'.");
    }
}

