namespace AppCloudBlog.Application.Features.Comments;

public record GetCommentsByPostQuery(Guid PostId) : IRequest<APIResponse>;

public class GetCommentsByPostQueryHandler(ICommentRepository commentRepo)
    : IRequestHandler<GetCommentsByPostQuery, APIResponse>
{
    public async Task<APIResponse> Handle(GetCommentsByPostQuery request, CancellationToken ct)
    {
        var comments = await commentRepo.GetByPostIdAsync(request.PostId);

        // Filter only approved comments and replies
        var approved = comments
            .Where(c => c.Status == "Approved")
            .Select(MapToDto)
            .ToList();

        return APIResponse.Ok(approved);
    }

    private static CommentDto MapToDto(Comment comment)
    {
        return new CommentDto(
            comment.Id,
            comment.Content,
            comment.Status,
            comment.CreatedAt,
            comment.User.Name,
            comment.ParentId,
            comment.Replies?
                .Where(r => r.Status == "Approved")
                .OrderBy(r => r.CreatedAt)
                .Select(MapToDto)
                .ToList()
        );
    }
}
