namespace AppCloudBlog.Application.Features.Auth;

public record LoginCommand(string Username, string Password) : IRequest<APIResponse>;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Username).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}

public class LoginCommandHandler(
    IAuthRepository authRepo,
    IPasswordHasher hasher,
    IJwtTokenGenerator jwtGenerator) : IRequestHandler<LoginCommand, APIResponse>
{
    public async Task<APIResponse> Handle(LoginCommand request, CancellationToken ct)
    {
        var user = await authRepo.GetByEmailAsync(request.Username);
        if (user is null || !hasher.VerifyPasswordHash(request.Password, user.Password, user.PasswordSalt))
        {
            return new APIResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                IsSuccess = false,
                ErrorMessages = ["Invalid username or password."]
            };
        }

        var token = jwtGenerator.GenerateToken(user);

        return new APIResponse
        {
            StatusCode = HttpStatusCode.OK,
            IsSuccess = true,
            Result = new LoginResponseDto(user.Adapt<UserDto>(), token)
        };
    }
}
