namespace Lex.SharedKernel.Primitives;

/// <summary>
/// Railway-oriented result. All handlers return Result&lt;T&gt;.
/// Exceptions are reserved for unrecoverable infrastructure failures.
/// </summary>
public sealed record Result<T>
{
    public T? Value { get; }
    public Error? Error { get; }
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    private Result(T value)     { Value = value; IsSuccess = true; }
    private Result(Error error) { Error = error; IsSuccess = false; }

    public static Result<T> Success(T value)     => new(value);
    public static Result<T> Failure(Error error) => new(error);
    public static implicit operator Result<T>(T value)     => Success(value);
    public static implicit operator Result<T>(Error error) => Failure(error);
}

public sealed record Error(string Code, string Message, ErrorType Type = ErrorType.Failure)
{
    public static Error NotFound(string code, string message)   => new(code, message, ErrorType.NotFound);
    public static Error Validation(string code, string message) => new(code, message, ErrorType.Validation);
    public static Error Conflict(string code, string message)   => new(code, message, ErrorType.Conflict);
    public static Error Unauthorized(string code, string message) => new(code, message, ErrorType.Unauthorized);
    public static Error Failure(string code, string message) => new(code, message, ErrorType.Failure);
}

public sealed record Result
{
    public Error? Error { get; }
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    private Result(bool isSuccess, Error? error) { IsSuccess = isSuccess; Error = error; }

    public static Result Success()               => new(true, null);
    public static Result Failure(Error error)    => new(false, error);
    public static implicit operator Result(Error error) => Failure(error);
}

public enum ErrorType { Failure, NotFound, Validation, Conflict, Unauthorized }
