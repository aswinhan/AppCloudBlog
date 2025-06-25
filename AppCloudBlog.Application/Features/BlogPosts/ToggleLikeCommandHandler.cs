namespace AppCloudBlog.Application.Features.BlogPosts;

public record ToggleLikeCommand(string SlugOrId, ClaimsPrincipal User) : IRequest<APIResponse>;

public class ToggleLikeCommandHandler(IBlogPostRepository blogRepo)
    : IRequestHandler<ToggleLikeCommand, APIResponse>
{
    public async Task<APIResponse> Handle(ToggleLikeCommand request, CancellationToken ct)
    {
        BlogPost? post;

        if (Guid.TryParse(request.SlugOrId, out var id))
            post = await blogRepo.GetByIdAsync(id);
        else
            post = await blogRepo.GetBySlugAsync(request.SlugOrId);

        if (post is null || post.IsDeleted || post.Status != "Published")
            return APIResponse.Fail("Post not found or not published", HttpStatusCode.NotFound);

        var userId = request.User.GetUserId();

        var hasLiked = await blogRepo.HasUserLikedAsync(post.Id, userId);

        if (hasLiked)
            await blogRepo.RemoveLikeAsync(post.Id, userId);
        else
            await blogRepo.AddLikeAsync(post.Id, userId);

        var totalLikes = await blogRepo.GetLikeCountAsync(post.Id);

        return APIResponse.Ok(new
        {
            postId = post.Id,
            liked = !hasLiked,
            likeCount = totalLikes
        });
    }
}

