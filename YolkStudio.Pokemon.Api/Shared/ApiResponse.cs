namespace YolkStudio.Pokemon.Api.Shared;

public record ApiResponse<T>(string Message, T? Data = default);

public record ValidationErrorResponse(string Message, IEnumerable<string>? Errors = null);

public record ErrorResponse(string Message);