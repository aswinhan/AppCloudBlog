namespace AppCloudBlog.Application.Features.Auth;

public record GetCurrentUserCommand(ClaimsPrincipal User) : IRequest<APIResponse>;

public class GetCurrentUserCommandHandler(IAuthRepository authRepo)
    : IRequestHandler<GetCurrentUserCommand, APIResponse>
{
    public async Task<APIResponse> Handle(GetCurrentUserCommand request, CancellationToken ct)
    {
        var userId = request.User.GetUserId();

        var user = await authRepo.GetByIdAsync(userId);
        if (user is null)
        {
            return new APIResponse
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.NotFound,
                ErrorMessages = ["User not found."]
            };
        }

        return new APIResponse
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Result = user.Adapt<UserDto>()
        };
    }
}
