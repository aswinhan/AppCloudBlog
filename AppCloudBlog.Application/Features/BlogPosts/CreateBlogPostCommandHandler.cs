using AppCloudBlog.Application.Common.Helpers;
using AppCloudBlog.Application.Common.Interfaces.Services;
using AppCloudBlog.Application.DTOs.Blog;

namespace AppCloudBlog.Application.Features.BlogPosts;

public record CreateBlogPostCommand(
    string Title,
    string Content,
    List<string> Tags,
    string? CoverImageUrl,
    DateTime? PublishAt,
    ClaimsPrincipal User,
    string? Status = null
    ) : IRequest<APIResponse>;


public class CreateBlogPostCommandValidator : AbstractValidator<CreateBlogPostCommand>
{
    public CreateBlogPostCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Content).NotEmpty();
        RuleFor(x => x.Tags).NotNull();
    }
}


public class CreateBlogPostCommandHandler(
    IBlogPostRepository blogRepo,
    ITagRepository tagRepo,
    INotificationService notificationService
) : IRequestHandler<CreateBlogPostCommand, APIResponse>
{
    public async Task<APIResponse> Handle(CreateBlogPostCommand request, CancellationToken ct)
{
    var slug = SlugHelper.GenerateSlug(request.Title);

    if (!await blogRepo.IsSlugUniqueAsync(slug))
    {
        return new APIResponse
        {
            IsSuccess = false,
            StatusCode = HttpStatusCode.BadRequest,
            ErrorMessages = [$"Slug '{slug}' already exists."]
        };
    }

    var authorId = request.User.GetUserId();

    var tagEntities = await tagRepo.ResolveTagsAsync(request.Tags);

    var blogPost = new BlogPost
    {
        Title = request.Title,
        Content = request.Content,
        CoverImageUrl = request.CoverImageUrl,
        Slug = slug,
        Status = request.Status ?? "Draft", // Allow optional override
        AuthorId = authorId,
        PublishAt = request.PublishAt ?? DateTime.UtcNow,
        PostTags = tagEntities.Select(tag => new PostTag { Tag = tag }).ToList()
    };

    await blogRepo.AddAsync(blogPost);

    // 🔔 Notify followers if created as published
    if (blogPost.Status == "Published")
    {
        var author = new User { Id = authorId }; // You may load full User if needed
        await notificationService.NotifyFollowersOnNewPostAsync(author, blogPost);
    }

    return new APIResponse
    {
        IsSuccess = true,
        StatusCode = HttpStatusCode.Created,
        Result = blogPost.Adapt<BlogPostDto>()
    };
}

}


