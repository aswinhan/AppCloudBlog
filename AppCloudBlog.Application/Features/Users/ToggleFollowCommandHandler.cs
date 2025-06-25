namespace AppCloudBlog.Application.Features.Users;

public record ToggleFollowCommand(string TargetUsername, ClaimsPrincipal User) : IRequest<APIResponse>;

public class ToggleFollowCommandHandler(IAuthRepository authRepo)
    : IRequestHandler<ToggleFollowCommand, APIResponse>
{
    public async Task<APIResponse> Handle(ToggleFollowCommand request, CancellationToken ct)
    {
        var followerId = request.User.GetUserId();

        var targetUser = await authRepo.GetByUsernameAsync(request.TargetUsername);
        if (targetUser is null || targetUser.Id == followerId)
            return APIResponse.Fail("Invalid target user", HttpStatusCode.BadRequest);

        var isFollowing = await authRepo.IsFollowingAsync(followerId, targetUser.Id);

        if (isFollowing)
            await authRepo.UnfollowAsync(followerId, targetUser.Id);
        else
            await authRepo.FollowAsync(followerId, targetUser.Id);

        return APIResponse.Ok(new { following = !isFollowing });
    }
}

