namespace YolkStudio.Pokemon.Core.Extensions;

public static class DateTimeExtensions
{
    public static DateTimeOffset ToUtcDateTimeOffset(this DateTime utcDateTime)
    {
        return new DateTimeOffset(utcDateTime, TimeSpan.Zero);
    }
}