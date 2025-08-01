using ECommerceSystem.Shared.DTOs;
using ECommerceSystem.Shared.DTOs.Pet;
using Refit;

namespace ECommerceSystem.GUI.Apis
{
    public interface IUserPetApi
    {
        [Post("/api/user-pet-service/adopt/{petId}")]
        Task<ApiResponseDto> AdoptPetAsync(int petId);

        [Post("/api/user-pet-service/toggle-like/{petId}")]
        Task<ApiResult<bool>> ToggleLikeAsync(int petId);

        [Get("/api/user-pet-service/favorites")]
        Task<ApiResult<PetListDto[]>> GetUserFavoritesAsync();

        [Get("/api/user-pet-service/adoptions")]
        Task<ApiResult<PetListDto[]>> GetUserAdoptionsAsync();
    }
}
