namespace AppCloudBlog.Application.Features.BlogPosts;

public record ListBookmarkedPostsQuery(ClaimsPrincipal User) : IRequest<APIResponse>;

public class ListBookmarkedPostsQueryHandler(IBlogPostRepository blogRepo)
    : IRequestHandler<ListBookmarkedPostsQuery, APIResponse>
{
    public async Task<APIResponse> Handle(ListBookmarkedPostsQuery request, CancellationToken ct)
    {
        var userId = request.User.GetUserId();

        var posts = await blogRepo.GetBookmarkedPostsAsync(userId);

        return APIResponse.Ok(posts.Select(p => p.Adapt<BlogPostDto>()).ToList());
    }
}

