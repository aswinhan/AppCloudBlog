using AppCloudBlog.Application.Common.Helpers;
using AppCloudBlog.Application.DTOs.Blog;

namespace AppCloudBlog.Application.Features.BlogPosts;

public record CreateBlogPostCommand(
    string Title,
    string Content,
    List<string> Tags,
    string? CoverImageUrl,
    DateTime? PublishAt,
    ClaimsPrincipal User) : IRequest<APIResponse>;


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
    ITagRepository tagRepo) : IRequestHandler<CreateBlogPostCommand, APIResponse>
{
    public async Task<APIResponse> Handle(CreateBlogPostCommand request, CancellationToken ct)
    {
        var slug = SlugHelper.GenerateSlug(request.Title);

        // Ensure unique slug
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

        // Resolve tag entities
        var tagEntities = await tagRepo.ResolveTagsAsync(request.Tags);

        var blogPost = new BlogPost
        {
            Title = request.Title,
            Content = request.Content,
            CoverImageUrl = request.CoverImageUrl,
            Slug = slug,
            Status = "Draft", // Default status
            AuthorId = authorId,
            PublishAt = request.PublishAt ?? DateTime.UtcNow,
            PostTags = tagEntities.Select(tag => new PostTag { Tag = tag }).ToList()
        };

        await blogRepo.AddAsync(blogPost);

        return new APIResponse
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.Created,
            Result = blogPost.Adapt<BlogPostDto>()
        };
    }
}


