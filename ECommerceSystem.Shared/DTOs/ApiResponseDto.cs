namespace ECommerceSystem.Shared.DTOs
{
    public class ApiResponseDto
    {
        public bool Success { get; set; }

        public string Message { get; set; } = string.Empty;

        public List<string> Errors { get; set; }

        public static ApiResponseDto Ok(string message = "")
            => new() { Success = true, Message = message };

        public static ApiResponseDto Fail(string message)
            => new() { Success = false, Message = message };

        public static ApiResponseDto Fail(List<string> errors, string message = "Dữ liệu không hợp lệ.")
            => new() { Success = false, Message = message, Errors = errors };
    }

}
