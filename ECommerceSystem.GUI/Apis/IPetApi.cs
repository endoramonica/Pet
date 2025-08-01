using ECommerceSystem.Shared.DTOs;
using ECommerceSystem.Shared.DTOs.Pet;
using Refit;

namespace ECommerceSystem.GUI.Apis
{
    public interface IPetApi
    {
        [Get("/api/pet/new/{count}")]
        Task<ApiResult<PetListDto[]>> GetNewlyAddedPetsAsync(int count);

        [Get("/api/pet/popular/{count}")]
        Task<ApiResult<PetListDto[]>> GetPopularPetsAsync(int count);

        [Get("/api/pet")]
        Task<ApiResult<PetListDto[]>> GetAllPetsAsync();

        [Get("/api/pet/random/{count}")]
        Task<ApiResult<PetListDto[]>> GetRandomPetsAsync(int count);

        [Get("/api/pet/{id}")]
        Task<ApiResult<PetDetailDto>> GetPetDetailsAsync(int id);

        [Get("/api/admin/pets")]
        Task<ApiResult<PetListDto[]>> GetAllPetsForAdminAsync();

        [Get("/api/admin/pets/for-update/{id}")]
        Task<ApiResult<PetCreateUpdateDto>> GetPetForUpdateAsync(int id);

        [Post("/api/admin/pets")]
        Task<ApiResult<int>> CreatePetAsync([Body] PetCreateUpdateDto petDto);

        [Put("/api/admin/pets/{id}")]
        Task<ApiResult<bool>> UpdatePetAsync(int id, [Body] PetCreateUpdateDto petDto);

        [Delete("/api/admin/pets/{id}")]
        Task<ApiResult<bool>> DeletePetAsync(int id);
    }
}
