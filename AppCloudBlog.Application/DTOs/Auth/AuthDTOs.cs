namespace AppCloudBlog.Application.DTOs.Auth;

// Auth input DTOs
public record RegisterRequestDto(string Username, string Name, string Password);
public record LoginRequestDto(string Username, string Password);

// Auth output DTOs
public record LoginResponseDto(UserDto User, string Token);
public record AuthResponseDto(bool IsSuccess, string? Token, UserDto? User, List<string>? Errors = null);

// User info representation
public record UserDto(Guid Id, string Username, string Name, string Role, string? AvatarUrl = null);

