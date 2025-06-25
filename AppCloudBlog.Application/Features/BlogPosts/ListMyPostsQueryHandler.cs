namespace AppCloudBlog.Application.Features.BlogPosts;

public record ListMyPostsQuery(ClaimsPrincipal User, int Page = 1, int PageSize = 10, string? Status = null) : IRequest<APIResponse>;


public class ListMyPostsQueryHandler(IBlogPostRepository blogRepo)
    : IRequestHandler<ListMyPostsQuery, APIResponse>
{
    public async Task<APIResponse> Handle(ListMyPostsQuery request, CancellationToken ct)
    {
        var authorId = request.User.GetUserId();

        var (posts, total) = await blogRepo.GetAuthorPagedAsync(authorId, request.Page, request.PageSize, request.Status);

        var enrichedPosts = new List<BlogPostDto>();

        foreach (var post in posts)
        {
            var dto = post.Adapt<BlogPostDto>();

            dto = dto with
            {
                LikeCount = await blogRepo.GetLikeCountAsync(post.Id),
                LikedByMe = await blogRepo.HasUserLikedAsync(post.Id, authorId),
                BookmarkedByMe = await blogRepo.IsBookmarkedAsync(post.Id, authorId)
            };

            enrichedPosts.Add(dto);
        }

        return APIResponse.Ok(new PagedResult<BlogPostDto>(
            enrichedPosts, total, request.Page, request.PageSize
        ));
    }
}

