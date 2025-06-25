namespace AppCloudBlog.Application.Features.Users;

public record GetFollowersQuery(string Username) : IRequest<APIResponse>;

public class GetFollowersQueryHandler(IAuthRepository authRepo)
    : IRequestHandler<GetFollowersQuery, APIResponse>
{
    public async Task<APIResponse> Handle(GetFollowersQuery request, CancellationToken ct)
    {
        var user = await authRepo.GetByUsernameAsync(request.Username);
        if (user is null)
            return APIResponse.Fail("User not found", HttpStatusCode.NotFound);

        var followers = await authRepo.GetFollowersAsync(user.Id);

        var dto = followers.Select(f => new AuthorSummaryDto(
            f.Id, f.Username, f.Name, f.AvatarUrl
        )).ToList();

        return APIResponse.Ok(dto);
    }
}


