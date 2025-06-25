namespace AppCloudBlog.Application.Features.Comments;

public record GetMyCommentHistoryQuery(ClaimsPrincipal User) : IRequest<APIResponse>;

public class GetMyCommentHistoryQueryHandler(ICommentRepository commentRepo)
    : IRequestHandler<GetMyCommentHistoryQuery, APIResponse>
{
    public async Task<APIResponse> Handle(GetMyCommentHistoryQuery request, CancellationToken ct)
    {
        var userId = request.User.GetUserId();

        var comments = await commentRepo.GetByUserIdAsync(userId);

        var dto = comments.Select(c => new CommentDto(
            c.Id,
            c.Content,
            c.Status,
            c.CreatedAt,
            c.User.Name,
            c.ParentId,
            null // No replies needed in flat history
        )).ToList();

        return APIResponse.Ok(dto);
    }
}
