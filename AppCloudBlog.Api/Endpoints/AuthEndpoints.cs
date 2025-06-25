namespace AppCloudBlog.Api.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this RouteGroupBuilder group)
    {
        group.MapPost("/login", async (ISender sender, LoginCommand command) =>
        {
            var result = await sender.Send(command);
            return Results.Json(result, statusCode: (int)result.StatusCode);
        })
        .WithName("Login")
        .WithTags("Auth")
        .Accepts<LoginCommand>("application/json")
        .Produces<APIResponse>(200)
        .Produces<APIResponse>(400)
        .AllowAnonymous();

        group.MapPost("/register", async (ISender sender, RegisterCommand command) =>
        {
            var result = await sender.Send(command);
            return Results.Json(result, statusCode: (int)result.StatusCode);
        })
        .WithName("Register")
        .WithTags("Auth")
        .Accepts<RegisterCommand>("application/json")
        .Produces<APIResponse>(200)
        .Produces<APIResponse>(400)
        .AllowAnonymous();

        group.MapPost("/refresh", async (ISender sender, RefreshTokenCommand command) =>
        {
            var result = await sender.Send(command);
            return Results.Json(result, statusCode: (int)result.StatusCode);
        })
        .WithName("RefreshToken")
        .WithTags("Auth")
        .Accepts<RefreshTokenCommand>("application/json")
        .Produces<APIResponse>(200)
        .Produces<APIResponse>(401)
        .AllowAnonymous();

        group.MapPost("/logout", async (HttpContext ctx, ISender sender) =>
        {
            var result = await sender.Send(new LogoutCommand(ctx.User));
            return Results.Json(result, statusCode: (int)result.StatusCode);
        })
        .WithName("Logout")
        .WithTags("Auth")
        .Produces<APIResponse>(200)
        .Produces<APIResponse>(401)
        .RequireAuthorization();

        group.MapGet("/me", async (HttpContext ctx, ISender sender) =>
        {
            var result = await sender.Send(new GetCurrentUserQuery(ctx.User));
            return Results.Json(result, statusCode: (int)result.StatusCode);
        })
        .WithName("GetCurrentUser")
        .WithTags("Auth")
        .Produces<APIResponse>(200)
        .Produces<APIResponse>(401)
        .RequireAuthorization();
    }
}

