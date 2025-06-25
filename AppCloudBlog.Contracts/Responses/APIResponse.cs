using System.Net;

namespace AppCloudBlog.Contracts.Responses;

public record APIResponse
{
    public bool IsSuccess { get; init; }
    public object? Result { get; init; }
    public HttpStatusCode StatusCode { get; init; }
    public List<string> ErrorMessages { get; init; } = [];

    // Static helpers for clean usage
    public static APIResponse Ok(object? result = null)
    {
        return new APIResponse
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Result = result
        };
    }

    public static APIResponse Created(object? result = null)
    {
        return new APIResponse
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.Created,
            Result = result
        };
    }

    public static APIResponse Fail(string error, HttpStatusCode status = HttpStatusCode.BadRequest)
    {
        return new APIResponse
        {
            IsSuccess = false,
            StatusCode = status,
            ErrorMessages = [error]
        };
    }

    public static APIResponse Fail(List<string> errors, HttpStatusCode status = HttpStatusCode.BadRequest)
    {
        return new APIResponse
        {
            IsSuccess = false,
            StatusCode = status,
            ErrorMessages = errors
        };
    }
}
