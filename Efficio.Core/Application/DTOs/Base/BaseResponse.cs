namespace Efficio.Core.Application.DTOs.Base;

public class BaseResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}

public class BaseResponse<T> : BaseResponse
{
    public T? Data { get; set; }
    
    public static BaseResponse<T> SuccessResult(T data, string message = "")
    {
        return new BaseResponse<T>
        {
            Success = true,
            Message = message,
            Data = data
        };
    }
    
    public static BaseResponse<T> FailResult(string message)
    {
        return new BaseResponse<T>
        {
            Success = false,
            Message = message,
            Data = default
        };
    }
}