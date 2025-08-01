using ECommerceSystem.Shared.DTOs.Product;
using Refit;

namespace ECommerceSystem.GUI.Apis
{
    // Admin API
    public interface IAdminApi
    {
        [Get("/api/admin/statistics")]
        Task<StatisticDTO> GetStatisticsAsync([Query] string type, [Query] string? period = null);

        [Get("/api/admin/inventory")]
        Task<ApiInventoryResponse> GetInventoryAsync();

        [Get("/api/admin/user-activity")]
        Task<ApiUserActivityResponse> GetUserActivityAsync();
    }
}
