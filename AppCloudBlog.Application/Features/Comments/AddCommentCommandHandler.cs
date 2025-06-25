namespace AppCloudBlog.Application.Features.Comments;

public record AddCommentCommand(
    string Content,
    Guid PostId,
    Guid? ParentId,
    ClaimsPrincipal User
) : IRequest<APIResponse>;

public class AddCommentCommandValidator : AbstractValidator<AddCommentCommand>
{
    public AddCommentCommandValidator()
    {
        RuleFor(x => x.Content).NotEmpty().MaximumLength(1000);
        RuleFor(x => x.PostId).NotEmpty();
    }
}

public class AddCommentCommandHandler(
    IBlogPostRepository blogRepo,
    ICommentRepository commentRepo)
    : IRequestHandler<AddCommentCommand, APIResponse>
{
    public async Task<APIResponse> Handle(AddCommentCommand request, CancellationToken ct)
    {
        var post = await blogRepo.GetByIdAsync(request.PostId);

        if (post is null || post.IsDeleted || post.Status != "Published")
        {
            return APIResponse.Fail("Cannot comment on an unpublished or non-existent post", HttpStatusCode.BadRequest);
        }

        var comment = new Comment
        {
            Content = request.Content,
            PostId = request.PostId,
            UserId = request.User.GetUserId(),
            ParentId = request.ParentId,
            Status = "Pending",
            CreatedAt = DateTime.UtcNow
        };

        await commentRepo.AddAsync(comment);

        return APIResponse.Created(new { comment.Id, comment.Status });
    }
}

