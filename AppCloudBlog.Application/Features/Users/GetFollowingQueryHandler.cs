namespace AppCloudBlog.Application.Features.Users;

public record GetFollowingQuery(ClaimsPrincipal User) : IRequest<APIResponse>;

public class GetFollowingQueryHandler(IAuthRepository authRepo)
    : IRequestHandler<GetFollowingQuery, APIResponse>
{
    public async Task<APIResponse> Handle(GetFollowingQuery request, CancellationToken ct)
    {
        var userId = request.User.GetUserId();

        var following = await authRepo.GetFollowingAsync(userId);

        var dto = following.Select(f => new AuthorSummaryDto(
            f.Id, f.Username, f.Name, f.AvatarUrl
        )).ToList();

        return APIResponse.Ok(dto);
    }
}

