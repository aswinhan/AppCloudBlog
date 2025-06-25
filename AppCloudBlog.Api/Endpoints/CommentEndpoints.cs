using Microsoft.AspNetCore.Mvc;

namespace AppCloudBlog.Api.Endpoints;

public static class CommentEndpoints
{
    public static void MapCommentEndpoints(this RouteGroupBuilder group)
    {
        group.MapPost("/comments", async (HttpContext ctx, ISender sender, AddCommentCommand command) =>
        {
            var enrichedCommand = command with { User = ctx.User };
            var result = await sender.Send(enrichedCommand);
            return Results.Json(result, statusCode: (int)result.StatusCode);
        })
        .WithName("AddComment")
        .WithTags("Comments")
        .Produces<APIResponse>(201)
        .Produces<APIResponse>(400)
        .RequireAuthorization();

        group.MapGet("/comments/post/{postId}", async (Guid postId, ISender sender) =>
        {
            var result = await sender.Send(new GetCommentsByPostQuery(postId));
            return Results.Json(result, statusCode: (int)result.StatusCode);
        })
        .WithName("GetCommentsByPost")
        .WithTags("Comments")
        .Produces<APIResponse>(200)
        .AllowAnonymous();

        group.MapPut("/comments/{commentId}/moderate", async (
            Guid commentId,
            [FromQuery] string action,
            ISender sender) =>
        {
            var command = new ApproveRejectCommentCommand(commentId, action);
            var result = await sender.Send(command);
            return Results.Json(result, statusCode: (int)result.StatusCode);
        })
        .WithName("ModerateComment")
        .WithTags("Comments")
        .Produces<APIResponse>(200)
        .Produces<APIResponse>(400)
        .Produces<APIResponse>(403)
        .RequireAuthorization("AdminOnly");

        group.MapGet("/comments/me", async (HttpContext ctx, ISender sender) =>
        {
            var result = await sender.Send(new GetMyCommentHistoryQuery(ctx.User));
            return Results.Json(result, statusCode: (int)result.StatusCode);
        })
        .WithName("GetMyCommentHistory")
        .WithTags("Comments")
        .Produces<APIResponse>(200)
        .RequireAuthorization();



    }
}

