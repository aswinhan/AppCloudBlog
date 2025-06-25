using AppCloudBlog.Application.Common.Interfaces.Services;

namespace AppCloudBlog.Application.Features.BlogPosts;

public record TogglePublishStatusCommand(string SlugOrId, ClaimsPrincipal User) : IRequest<APIResponse>;

public class TogglePublishStatusCommandHandler(
    IBlogPostRepository blogRepo,
    INotificationService notificationService
) : IRequestHandler<TogglePublishStatusCommand, APIResponse>
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

        var wasDraft = post.Status == "Draft";

        post.Status = post.Status == "Published" ? "Draft" : "Published";
        if (post.Status == "Published")
            post.PublishAt ??= DateTime.UtcNow;

        post.UpdatedAt = DateTime.UtcNow;

        await blogRepo.UpdateAsync(post);

        // 🔔 Notify followers only if switching from Draft → Published
        if (wasDraft && post.Status == "Published")
        {
            await notificationService.NotifyFollowersOnNewPostAsync(post.Author, post);
        }

        return APIResponse.Ok($"Post status updated to '{post.Status}'.");
    }

}

