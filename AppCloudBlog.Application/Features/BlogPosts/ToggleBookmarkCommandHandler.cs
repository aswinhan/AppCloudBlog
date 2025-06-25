namespace AppCloudBlog.Application.Features.BlogPosts;

public record ToggleBookmarkCommand(string SlugOrId, ClaimsPrincipal User) : IRequest<APIResponse>;

public class ToggleBookmarkCommandHandler(IBlogPostRepository blogRepo)
    : IRequestHandler<ToggleBookmarkCommand, APIResponse>
{
    public async Task<APIResponse> Handle(ToggleBookmarkCommand request, CancellationToken ct)
    {
        BlogPost? post;

        if (Guid.TryParse(request.SlugOrId, out var id))
            post = await blogRepo.GetByIdAsync(id);
        else
            post = await blogRepo.GetBySlugAsync(request.SlugOrId);

        if (post is null || post.IsDeleted || post.Status != "Published")
            return APIResponse.Fail("Post not found or not published", HttpStatusCode.NotFound);

        var userId = request.User.GetUserId();

        var already = await blogRepo.IsBookmarkedAsync(post.Id, userId);

        if (already)
            await blogRepo.RemoveBookmarkAsync(post.Id, userId);
        else
            await blogRepo.AddBookmarkAsync(post.Id, userId);

        return APIResponse.Ok(new
        {
            postId = post.Id,
            bookmarked = !already
        });
    }
}

