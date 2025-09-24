namespace YolkStudio.Pokemon.Core.Shared;

public record PagedResult<T>(IEnumerable<T> Items, int TotalCount, int PageNumber, int PageSize);