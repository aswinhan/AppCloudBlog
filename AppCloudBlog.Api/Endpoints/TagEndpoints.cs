using AppCloudBlog.Application.DTOs.Blog;
using AppCloudBlog.Application.Features.Tags;

namespace AppCloudBlog.Api.Endpoints;

public static class TagEndpoints
{
    public static void MapTagEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/tags", async (ISender sender) =>
        {
            var result = await sender.Send(new GetAllTagsQuery());
            return Results.Json(result, statusCode: (int)result.StatusCode);
        })
        .WithName("GetAllTags")
        .WithTags("Tags")
        .Produces<APIResponse>(200)
        .AllowAnonymous();
    }
}
