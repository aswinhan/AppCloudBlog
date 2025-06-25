namespace AppCloudBlog.Application.Features.Comments;

public record ApproveRejectCommentCommand(Guid CommentId, string Action) : IRequest<APIResponse>;

public class ApproveRejectCommentCommandValidator : AbstractValidator<ApproveRejectCommentCommand>
{
    public ApproveRejectCommentCommandValidator()
    {
        RuleFor(x => x.CommentId).NotEmpty();
        RuleFor(x => x.Action)
            .NotEmpty()
            .Must(action => action is "approve" or "reject")
            .WithMessage("Action must be either 'approve' or 'reject'");
    }
}

public class ApproveRejectCommentCommandHandler(ICommentRepository commentRepo)
    : IRequestHandler<ApproveRejectCommentCommand, APIResponse>
{
    public async Task<APIResponse> Handle(ApproveRejectCommentCommand request, CancellationToken ct)
    {
        var comment = await commentRepo.GetByIdAsync(request.CommentId);

        if (comment is null)
            return APIResponse.Fail("Comment not found", HttpStatusCode.NotFound);

        if (comment.Status == "Approved" || comment.Status == "Rejected")
            return APIResponse.Fail("Comment is already moderated", HttpStatusCode.Conflict);

        comment.Status = request.Action == "approve" ? "Approved" : "Rejected";
        comment.UpdatedAt = DateTime.UtcNow;

        await commentRepo.UpdateAsync(comment);

        return APIResponse.Ok(new
        {
            comment.Id,
            comment.Status
        });
    }
}

