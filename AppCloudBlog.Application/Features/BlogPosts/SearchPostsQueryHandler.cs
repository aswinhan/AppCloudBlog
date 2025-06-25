namespace AppCloudBlog.Application.Features.BlogPosts;

public record SearchPostsQuery(
    ClaimsPrincipal? User,
    string? Keyword,
    List<string>? Tags,
    int Page = 1,
    int PageSize = 10
) : IRequest<APIResponse>;


public class SearchPostsQueryHandler(IBlogPostRepository blogRepo)
    : IRequestHandler<SearchPostsQuery, APIResponse>
{
    public async Task<APIResponse> Handle(SearchPostsQuery request, CancellationToken ct)
    {
        var (posts, total) = await blogRepo.SearchPublicPostsAsync(
            request.Keyword, request.Tags, request.Page, request.PageSize);

        var isAuth = request.User?.Identity?.IsAuthenticated == true;
        var userId = isAuth ? request.User!.GetUserId() : Guid.Empty;

        // Enrich each post with like/bookmark info
        var enrichedPosts = new List<BlogPostDto>();

        foreach (var post in posts)
        {
            var dto = post.Adapt<BlogPostDto>();

            if (isAuth)
            {
                dto = dto with
                {
                    LikeCount = await blogRepo.GetLikeCountAsync(post.Id),
                    LikedByMe = await blogRepo.HasUserLikedAsync(post.Id, userId),
                    BookmarkedByMe = await blogRepo.IsBookmarkedAsync(post.Id, userId)
                };
            }
            else
            {
                dto = dto with
                {
                    LikeCount = await blogRepo.GetLikeCountAsync(post.Id)
                };
            }

            enrichedPosts.Add(dto);
        }

        var paged = new PagedResult<BlogPostDto>(
            enrichedPosts,
            total,
            request.Page,
            request.PageSize
        );

        return APIResponse.Ok(paged);
    }
}

