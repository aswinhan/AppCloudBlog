namespace AppCloudBlog.Application.Features.BlogPosts;

public record GetBlogPostBySlugOrIdQuery(string SlugOrId, ClaimsPrincipal? User) : IRequest<APIResponse>;

public class GetBlogPostBySlugOrIdQueryHandler(
    IBlogPostRepository blogRepo)
    : IRequestHandler<GetBlogPostBySlugOrIdQuery, APIResponse>
{
    public async Task<APIResponse> Handle(GetBlogPostBySlugOrIdQuery request, CancellationToken ct)
    {
        BlogPost? post;
        if (Guid.TryParse(request.SlugOrId, out var id))
            post = await blogRepo.GetByIdAsync(id);
        else
            post = await blogRepo.GetBySlugAsync(request.SlugOrId);

        if (post is null || post.IsDeleted)
            return APIResponse.Fail("Post not found", HttpStatusCode.NotFound);

        var dto = post.Adapt<BlogPostDto>();

        dto = dto with
        {
            LikeCount = await blogRepo.GetLikeCountAsync(post.Id),
            LikedByMe = request.User?.Identity?.IsAuthenticated == true &&
                        await blogRepo.HasUserLikedAsync(post.Id, request.User.GetUserId()),
            BookmarkedByMe = request.User?.Identity?.IsAuthenticated == true &&
                     await blogRepo.IsBookmarkedAsync(post.Id, request.User.GetUserId())
        };

        return APIResponse.Ok(dto);
    }
}

