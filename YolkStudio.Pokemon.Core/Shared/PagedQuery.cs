using System.Text.Json.Serialization;

namespace YolkStudio.Pokemon.Core.Shared;

public abstract record PagedQuery
{
    private const int MaxPageSize = 100;
    public int PageNumber { get; init; } = 1;

    private readonly int _pageSize = 20;
    public int PageSize
    {
        get => _pageSize;
        init => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SortDirection
{
    Asc,
    Desc,
}

public abstract record SortedQuery : PagedQuery
{
    public SortDirection SortDirection { get; init; } = SortDirection.Asc;
}