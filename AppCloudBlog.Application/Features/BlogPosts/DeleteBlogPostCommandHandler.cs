namespace AppCloudBlog.Application.Features.BlogPosts;

public record DeleteBlogPostCommand(string SlugOrId, ClaimsPrincipal User) : IRequest<APIResponse>;

public class DeleteBlogPostCommandHandler(
    IBlogPostRepository blogRepo)
    : IRequestHandler<DeleteBlogPostCommand, APIResponse>
{
    public async Task<APIResponse> Handle(DeleteBlogPostCommand request, CancellationToken ct)
    {
        BlogPost? post;

        if (Guid.TryParse(request.SlugOrId, out var id))
            post = await blogRepo.GetByIdAsync(id);
        else
            post = await blogRepo.GetBySlugAsync(request.SlugOrId);

        if (post is null || post.IsDeleted)
        {
            return APIResponse.Fail("Post not found", HttpStatusCode.NotFound);
        }

        var userId = request.User.GetUserId();
        if (post.AuthorId != userId)
        {
            return APIResponse.Fail("You are not authorized to delete this post", HttpStatusCode.Forbidden);
        }

        await blogRepo.DeleteAsync(post);

        return APIResponse.Ok("Blog post deleted (soft)");
    }
}

