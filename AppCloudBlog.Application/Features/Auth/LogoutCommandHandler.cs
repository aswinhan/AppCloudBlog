namespace AppCloudBlog.Application.Features.Auth;

public record LogoutCommand(ClaimsPrincipal User) : IRequest<APIResponse>;

public class LogoutCommandHandler(
    IRefreshTokenService refreshTokenService) : IRequestHandler<LogoutCommand, APIResponse>
{
    public async Task<APIResponse> Handle(LogoutCommand request, CancellationToken ct)
    {
        var userId = request.User.GetUserId();

        await refreshTokenService.RevokeAllAsync(userId);

        return new APIResponse
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Result = "Logged out successfully. All refresh tokens revoked."
        };
    }
}

