using AppCloudBlog.Application.DTOs.Blog;

namespace AppCloudBlog.Api.Endpoints;

public static class BlogPostEndpoints
{
    public static void MapBlogPostEndpoints(this RouteGroupBuilder group)
    {
        group.MapPost("/create", async (HttpContext ctx, ISender sender, CreateBlogPostCommand command) =>
        {
            // Inject ClaimsPrincipal into the command
            var enrichedCommand = command with { User = ctx.User };

            var result = await sender.Send(enrichedCommand);
            return Results.Json(result, statusCode: (int)result.StatusCode);
        })
        .WithName("CreateBlogPost")
        .WithTags("BlogPosts")
        .Accepts<CreateBlogPostCommand>("application/json")
        .Produces<APIResponse>(201)
        .Produces<APIResponse>(400)
        .RequireAuthorization();

        group.MapGet("/{slugOrId}", async (HttpContext ctx, ISender sender, string slugOrId) =>
        {
            var result = await sender.Send(new GetBlogPostBySlugOrIdQuery(slugOrId, ctx.User));
            return Results.Json(result, statusCode: (int)result.StatusCode);
        })
        .WithName("GetBlogPostBySlugOrId")
        .WithTags("BlogPosts")
        .Produces<APIResponse>(200)
        .Produces<APIResponse>(404)
        .AllowAnonymous();

        group.MapPut("/update/{slugOrId}", async (HttpContext ctx, ISender sender, string slugOrId, UpdateBlogPostRequestDto dto) =>
        {
            var command = new UpdateBlogPostCommand(
                slugOrId,
                dto.Title,
                dto.Content,
                dto.Tags,
                dto.CoverImageUrl,
                dto.PublishAt,
                ctx.User
            );

            var result = await sender.Send(command);
            return Results.Json(result, statusCode: (int)result.StatusCode);
        })
        .WithName("UpdateBlogPost")
        .WithTags("BlogPosts")
        .Produces<APIResponse>(200)
        .Produces<APIResponse>(403)
        .Produces<APIResponse>(404)
        .RequireAuthorization();

        group.MapDelete("/delete/{slugOrId}", async (HttpContext ctx, ISender sender, string slugOrId) =>
        {
            var command = new DeleteBlogPostCommand(slugOrId, ctx.User);
            var result = await sender.Send(command);
            return Results.Json(result, statusCode: (int)result.StatusCode);
        })
        .WithName("DeleteBlogPost")
        .WithTags("BlogPosts")
        .Produces<APIResponse>(200)
        .Produces<APIResponse>(403)
        .Produces<APIResponse>(404)
        .RequireAuthorization();

        group.MapPut("/toggle-status/{slugOrId}", async (HttpContext ctx, ISender sender, string slugOrId) =>
        {
            var result = await sender.Send(new TogglePublishStatusCommand(slugOrId, ctx.User));
            return Results.Json(result, statusCode: (int)result.StatusCode);
        })
        .WithName("TogglePublishStatus")
        .WithTags("BlogPosts")
        .Produces<APIResponse>(200)
        .Produces<APIResponse>(403)
        .Produces<APIResponse>(404)
        .RequireAuthorization();

        // Public list with query params
        group.MapGet("/", async (
            HttpContext ctx,
            [FromQuery] int page,
            [FromQuery] int pageSize,
            ISender sender) =>
        {
            var result = await sender.Send(new ListPublicPostsQuery(ctx.User, page, pageSize));
            return Results.Json(result, statusCode: (int)result.StatusCode);
        })
        .WithName("ListPublicPosts")
        .WithTags("BlogPosts")
        .Produces<APIResponse>(200)
        .AllowAnonymous();

        // Author list with filters
        group.MapGet("/mine", async (
            HttpContext ctx,
            [FromQuery] int page,
            [FromQuery] int pageSize,
            [FromQuery] string? status,
            ISender sender) =>
        {
            var result = await sender.Send(new ListMyPostsQuery(ctx.User, page, pageSize, status));
            return Results.Json(result, statusCode: (int)result.StatusCode);
        })
        .WithName("ListMyPosts")
        .WithTags("BlogPosts")
        .Produces<APIResponse>(200)
        .RequireAuthorization();

        group.MapGet("/search", async (
            HttpContext ctx,
            [FromQuery] string? keyword,
            [FromQuery(Name = "tags")] string? tagsCsv,
            [FromQuery] int page,
            [FromQuery] int pageSize,
            ISender sender) =>
        {
            var tags = string.IsNullOrWhiteSpace(tagsCsv)
        ? [] : tagsCsv.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();

            var result = await sender.Send(new SearchPostsQuery(ctx.User, keyword, tags, page, pageSize));
            return Results.Json(result, statusCode: (int)result.StatusCode);
        })
        .WithName("SearchPosts")
        .WithTags("BlogPosts")
        .Produces<APIResponse>(200)
        .AllowAnonymous();

        group.MapPost("/like/{slugOrId}", async (HttpContext ctx, ISender sender, string slugOrId) =>
        {
            var result = await sender.Send(new ToggleLikeCommand(slugOrId, ctx.User));
            return Results.Json(result, statusCode: (int)result.StatusCode);
        })
        .WithName("ToggleLikePost")
        .WithTags("BlogPosts")
        .Produces<APIResponse>(200)
        .RequireAuthorization();

        group.MapPost("/bookmark/{slugOrId}", async (HttpContext ctx, ISender sender, string slugOrId) =>
        {
            var result = await sender.Send(new ToggleBookmarkCommand(slugOrId, ctx.User));
            return Results.Json(result, statusCode: (int)result.StatusCode);
        })
        .WithName("ToggleBookmark")
        .WithTags("BlogPosts")
        .Produces<APIResponse>(200)
        .RequireAuthorization();

        group.MapGet("/bookmarks", async (HttpContext ctx, ISender sender) =>
        {
            var result = await sender.Send(new ListBookmarkedPostsQuery(ctx.User));
            return Results.Json(result, statusCode: (int)result.StatusCode);
        })
        .WithName("ListBookmarkedPosts")
        .WithTags("BlogPosts")
        .Produces<APIResponse>(200)
        .RequireAuthorization();


    }
}
