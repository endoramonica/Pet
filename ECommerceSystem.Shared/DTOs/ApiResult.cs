using System;
using System.Collections.Generic;

namespace ECommerceSystem.Shared.DTOs
{
    public class ApiResult<T>
    {
        public bool Success { get; set; }

        public string Message { get; set; } = string.Empty;

        public T Data { get; set; }

        // Danh sách lỗi (nếu có)
        public List<string> Errors { get; set; }

        public static ApiResult<T> Ok(T data, string message = "") =>
            new() { Success = true, Data = data, Message = message };

        public static ApiResult<T> Fail(string message) =>
            new() { Success = false, Message = message };

        public static ApiResult<T> Fail(List<string> errors, string message = "Dữ liệu không hợp lệ.") =>
            new() { Success = false, Message = message, Errors = errors };
    }

}
