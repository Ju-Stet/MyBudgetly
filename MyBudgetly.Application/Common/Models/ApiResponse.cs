namespace MyBudgetly.Application.Common.Models;

public class ApiResponse<T>
{
    public bool Success { get; init; }
    public string? Message { get; init; }
    public T? Data { get; init; }
    public int StatusCode { get; init; }

    public static ApiResponse<T> SuccessResponse(T data, int statusCode = 200, string? message = null)
        => new() { Success = true, Data = data, StatusCode = statusCode, Message = message };

    public static ApiResponse<T> Failure(string message, int statusCode)
        => new() { Success = false, Data = default, StatusCode = statusCode, Message = message };
}
