using ECommerceSystem.Shared.DTOs.Models;
using ECommerceSystem.Shared.DTOs.User;
using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerceSystem.GUI.Apis
{
    // Auth API
    public interface IAuthApi
    {
        [Post("/api/auth/login")]
        Task<LoginResponse> Login([Body] LoginModel model);

        [Post("/api/auth/register")]
        Task<RegisterResponse> Register([Body] RegisterModel model);

        [Get("/api/auth/role")]
        Task<RoleResponse> GetCurrentRole();

        [Post("/api/auth/refresh")]
        Task<RefreshTokenResponse> Refresh([Body] RefreshTokenRequest model);
    }

    public class ApiInventoryResponse
    {
        public List<object> LowStock { get; set; } = new();
    }

    public class ApiUserActivityResponse
    {
        public List<object> Activities { get; set; } = new();
    }
}
