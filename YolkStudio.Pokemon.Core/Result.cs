namespace YolkStudio.Pokemon.Core;

public class Result
{
    public bool IsSuccess { get; }
    public ErrorType? ErrorType { get; }
    public string? Message { get; }
    public bool IsError => !IsSuccess;

    protected Result(bool isSuccess, ErrorType? type, string? message)
    {
        IsSuccess = isSuccess;
        ErrorType = type;
        Message = message;
    }

    public static Result Ok() => new(true, null, null);
    public static Result Fail(ErrorType type, string message) => new(false, type, message);
}

public class Result<T> : Result
{
    public T? Value { get; }

    private Result(bool isSuccess, T? value, ErrorType? type, string? message)
        : base(isSuccess, type, message)
    {
        Value = value;
    }

    public static Result<T> Ok(T value) => new(true, value, null, null);
    public new static Result<T> Fail(ErrorType type, string message) => new(false, default, type, message);
}

public enum ErrorType
{
    Failure,
    Unexpected,
    Validation,
    Conflict,
    NotFound,
    Unauthorized,
    Forbidden,
}