namespace AppCloudBlog.Application.Features.Auth;

public record RefreshTokenCommand(string Token) : IRequest<APIResponse>;

public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(x => x.Token).NotEmpty();
    }
}

public class RefreshTokenCommandHandler(
    IRefreshTokenService tokenService,
    IJwtTokenGenerator jwtGenerator) : IRequestHandler<RefreshTokenCommand, APIResponse>
{
    public async Task<APIResponse> Handle(RefreshTokenCommand request, CancellationToken ct)
    {
        var token = await tokenService.GetValidTokenAsync(request.Token);
        if (token is null)
        {
            return new APIResponse
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.Unauthorized,
                ErrorMessages = ["Invalid or expired refresh token"]
            };
        }

        await tokenService.RevokeAsync(token);
        var newToken = await tokenService.CreateForUserAsync(token.User);
        var jwt = jwtGenerator.GenerateToken(token.User);

        return new APIResponse
        {
            StatusCode = HttpStatusCode.OK,
            IsSuccess = true,
            Result = new
            {
                AccessToken = jwt,
                RefreshToken = newToken.Token
            }
        };
    }
}

