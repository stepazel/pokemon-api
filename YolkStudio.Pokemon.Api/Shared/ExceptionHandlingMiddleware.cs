using System.Net;
using Microsoft.AspNetCore.Diagnostics;

namespace YolkStudio.Pokemon.Api.Shared;

public class CustomExceptionHandler : IExceptionHandler
{
    private readonly ILogger<CustomExceptionHandler> _logger;

    public CustomExceptionHandler(ILogger<CustomExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception ex,
        CancellationToken cancellationToken)
    {
        _logger.LogError(ex, "An unexpected error occured");
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";
        var response = new ErrorResponse("An unexpected error occured");
        await context.Response.WriteAsJsonAsync(response, cancellationToken: cancellationToken);
        return true;
    }
}