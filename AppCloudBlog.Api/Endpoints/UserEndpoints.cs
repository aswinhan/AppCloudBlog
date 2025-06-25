namespace AppCloudBlog.Api.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/users/{username}", async (
            HttpContext ctx,
            string username,
            ISender sender) =>
        {
            var result = await sender.Send(new GetAuthorProfileQuery(username, ctx.User));
            return Results.Json(result, statusCode: (int)result.StatusCode);
        })
        .WithName("GetPublicAuthorProfile")
        .WithTags("Users")
        .Produces<APIResponse>(200)
        .Produces<APIResponse>(404)
        .AllowAnonymous();

        group.MapPost("/users/{username}/follow", async (HttpContext ctx, string username, ISender sender) =>
        {
            var result = await sender.Send(new ToggleFollowCommand(username, ctx.User));
            return Results.Json(result, statusCode: (int)result.StatusCode);
        })
        .WithName("ToggleFollow")
        .WithTags("Users")
        .Produces<APIResponse>(200)
        .RequireAuthorization();

        group.MapGet("/users/{username}/followers", async (
            string username,
            ISender sender) =>
        {
            var result = await sender.Send(new GetFollowersQuery(username));
            return Results.Json(result, statusCode: (int)result.StatusCode);
        })
        .WithName("GetUserFollowers")
        .WithTags("Users")
        .Produces<APIResponse>(200)
        .AllowAnonymous();

        group.MapGet("/users/following", async (
            HttpContext ctx,
            ISender sender) =>
        {
            var result = await sender.Send(new GetFollowingQuery(ctx.User));
            return Results.Json(result, statusCode: (int)result.StatusCode);
        })
        .WithName("GetUserFollowing")
        .WithTags("Users")
        .Produces<APIResponse>(200)
        .RequireAuthorization();


    }
}

