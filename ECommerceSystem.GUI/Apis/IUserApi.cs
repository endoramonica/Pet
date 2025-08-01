using ECommerceSystem.Shared.DTOs.User;
using Refit;

namespace ECommerceSystem.GUI.Apis
{
    // User API
    public interface IUserApi
    {
        [Get("/api/admin/users")]
        Task<List<UserDTO>> GetAllAsync();

        [Get("/api/admin/users/{id}")]
        Task<UserDTO> GetByIdAsync(string id);

        [Post("/api/admin/users")]
        Task CreateAsync([Body] UserDTO dto);

        [Put("/api/admin/users/{id}")]
        Task UpdateAsync(string id, [Body] UserDTO dto);

        [Delete("/api/admin/users/{id}")]
        Task SoftDeleteAsync(string id);

        [Post("/api/admin/users/delete-multiple")]
        Task SoftDeleteMultipleAsync([Body] List<string> ids);

        [Get("/api/admin/users/search")]
        Task<List<UserDTO>> SearchByNameAsync([Query] string name);
    }
}
