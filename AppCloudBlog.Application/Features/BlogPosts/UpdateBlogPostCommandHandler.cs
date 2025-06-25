using AppCloudBlog.Application.Common.Helpers;

namespace AppCloudBlog.Application.Features.BlogPosts;

public record UpdateBlogPostCommand(
    string SlugOrId,
    string Title,
    string Content,
    List<string> Tags,
    string? CoverImageUrl,
    DateTime? PublishAt,
    ClaimsPrincipal User) : IRequest<APIResponse>;

public class UpdateBlogPostCommandValidator : AbstractValidator<UpdateBlogPostCommand>
{
    public UpdateBlogPostCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Content).NotEmpty();
        RuleFor(x => x.Tags).NotNull();
    }
}

public class UpdateBlogPostCommandHandler(
    IBlogPostRepository blogRepo,
    ITagRepository tagRepo)
    : IRequestHandler<UpdateBlogPostCommand, APIResponse>
{
    public async Task<APIResponse> Handle(UpdateBlogPostCommand request, CancellationToken ct)
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
            return APIResponse.Fail("You are not authorized to update this post", HttpStatusCode.Forbidden);
        }

        var tags = await tagRepo.ResolveTagsAsync(request.Tags);

        post.Title = request.Title;
        post.Content = request.Content;
        post.CoverImageUrl = request.CoverImageUrl;
        post.PublishAt = request.PublishAt ?? DateTime.UtcNow;
        post.UpdatedAt = DateTime.UtcNow;
        post.Slug = SlugHelper.GenerateSlug(request.Title);
        post.PostTags = tags.Select(t => new PostTag { Tag = t }).ToList();

        await blogRepo.UpdateAsync(post);

        return APIResponse.Ok(post.Adapt<BlogPostDto>());
    }
}

