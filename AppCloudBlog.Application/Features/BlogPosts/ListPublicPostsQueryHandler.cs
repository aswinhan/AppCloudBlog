namespace AppCloudBlog.Application.Features.BlogPosts;

public record ListPublicPostsQuery(ClaimsPrincipal? User, int Page = 1, int PageSize = 10) : IRequest<APIResponse>;


public class ListPublicPostsQueryHandler(IBlogPostRepository blogRepo)
    : IRequestHandler<ListPublicPostsQuery, APIResponse>
{
    public async Task<APIResponse> Handle(ListPublicPostsQuery request, CancellationToken ct)
    {
        var (posts, total) = await blogRepo.GetPublicPagedAsync(request.Page, request.PageSize);

        var isAuth = request.User?.Identity?.IsAuthenticated == true;
        var userId = isAuth ? request.User!.GetUserId() : Guid.Empty;

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
                dto = dto with { LikeCount = await blogRepo.GetLikeCountAsync(post.Id) };
            }

            enrichedPosts.Add(dto);
        }

        return APIResponse.Ok(new PagedResult<BlogPostDto>(
            enrichedPosts, total, request.Page, request.PageSize
        ));
    }
}


