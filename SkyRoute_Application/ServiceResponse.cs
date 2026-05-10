public class ServiceResponse<T>
{
    public T? Data { get; set; }
    public bool Success { get; set; } = true;
    public string Message { get; set; } = string.Empty;
    public static ServiceResponse<T> BuildError(string message)
        => new() { Success = false, Message = message };

    public static ServiceResponse<T> BuildSuccess(T data)
        => new() { Data = data };
}