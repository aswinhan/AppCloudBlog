namespace AppCloudBlog.Application.Features.Auth;

public record RegisterCommand(string Username, string Name, string Password) : IRequest<APIResponse>;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Username).NotEmpty().MinimumLength(3);
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
    }
}

public class RegisterCommandHandler(
    IAuthRepository authRepo,
    IPasswordHasher hasher) : IRequestHandler<RegisterCommand, APIResponse>
{
    public async Task<APIResponse> Handle(RegisterCommand request, CancellationToken ct)
    {
        if (!await authRepo.IsUniqueEmailAsync(request.Username))
        {
            return new APIResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                IsSuccess = false,
                ErrorMessages = [$"Username '{request.Username}' is already taken."]
            };
        }

        hasher.CreatePasswordHash(request.Password, out var passwordHash, out var passwordSalt);

        var user = new User
        {
            Username = request.Username,
            Name = request.Name,
            Email = request.Username, // assuming username doubles as email for now
            Password = passwordHash,
            PasswordSalt = passwordSalt,
            Role = "Reader"
        };

        var createdUser = await authRepo.CreateAsync(user);

        return new APIResponse
        {
            StatusCode = HttpStatusCode.OK,
            IsSuccess = true,
            Result = createdUser.Adapt<UserDto>()
        };
    }
}

