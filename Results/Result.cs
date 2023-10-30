namespace Results;


public record Result(bool Success, string? Message = default)
{
    public static readonly Result Ok = new(true);
    public static          Result Failed(string? error)                 => new(false, error);
    public static          Result Check(bool condition, string message) => condition ? Ok : Failed(message);

    public static implicit operator bool(Result result) => result.Success;

    public static async Task<Result> Try(Func<Task> action, Action<Exception>? callback = null)
    {
        try
        {
            await action();
            return Ok;
        }
        catch (Exception e)
        {
            callback?.Invoke(e);
            return Failed(e.Message);
        }
    }
}

public record Result<T>(bool Success, T? Value = default, string? Message = default)
    : Result(Success, Message)
{
    public new static Result<T> Failed(string? message)      => new(false, Message: message);
    public new static Result<T> Ok(T value)                  => new(true, value);
    public new static Result<T> Ok(T value, string? message) => new(true, value, message);
    
    public static implicit operator Result<T>(T value) => Ok(value);
    
    public static async Task<Result<T>> Try(Func<Task<T>> action, Action<Exception>? callback = null)
    {
        try
        {
            return await action();
        }
        catch (Exception e)
        {
            callback?.Invoke(e);
            return Failed(e.Message);
        }
    }

    public T TryGet(T defaultValue) => (Success ? Value : defaultValue)!;
}

public record Result<T, TError>(bool Success, T? Value, TError? Error = default, string? Message = default)
    : Result<T>(Success, Value, Message)
{
    public static     Result<T, TError> Failed(TError e)                 => new(false, default, e);
    public static     Result<T, TError> Failed(TError e, string message) => new(false, default, e, message);
    public new static Result<T, TError> Ok(T value)                      => new(true, value);
    public new static Result<T, TError> Ok(T value, string message)      => new(true, value, Message: message);

    public static implicit operator Result<T, TError>(T value) => Ok(value);
    public static implicit operator Result<T, TError>(TError error) => Failed(error);
    
    
    public static async Task<Result<T, Exception>> Try(Func<Task<T>> action)
    {
        try
        {
            return await action();
        }
        catch (Exception e)
        {
            return e;
        }
    }
}