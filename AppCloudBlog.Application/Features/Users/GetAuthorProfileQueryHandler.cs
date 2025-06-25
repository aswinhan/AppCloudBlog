namespace AppCloudBlog.Application.Features.Users;

public record GetAuthorProfileQuery(string Username, ClaimsPrincipal? Requester) : IRequest<APIResponse>;

public class GetAuthorProfileQueryHandler(IAuthRepository authRepo)
    : IRequestHandler<GetAuthorProfileQuery, APIResponse>
{
    public async Task<APIResponse> Handle(GetAuthorProfileQuery request, CancellationToken ct)
    {
        var user = await authRepo.GetByUsernameAsync(request.Username);

        if (user is null)
            return APIResponse.Fail("User not found", HttpStatusCode.NotFound);

        var followerCount = await authRepo.GetFollowerCountAsync(user.Id);

        var isFollowedByMe = false;

        if (request.Requester?.Identity?.IsAuthenticated == true)
        {
            var currentUserId = request.Requester.GetUserId();
            isFollowedByMe = await authRepo.IsFollowingAsync(currentUserId, user.Id);
        }

        var dto = new AuthorProfileDto(
            user.Id,
            user.Username,
            user.Name,
            user.AvatarUrl,
            user.BlogPosts.Count(p => p.Status == "Published" && !p.IsDeleted),
            isFollowedByMe,
            followerCount
        );

        return APIResponse.Ok(dto);
    }
}

