using System.Net;

namespace YolkStudio.Pokemon.Api.Shared;

public record ApiResponse<T>(
    HttpStatusCode StatusCode,
    string Message,
    T? Data = default,
    bool Success = true);

public record ErrorResponse(
    HttpStatusCode StatusCode,
    string Message,
    IEnumerable<string> Errors,
    bool Success = false);