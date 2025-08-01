using ECommerceSystem.Api.Services;
using ECommerceSystem.Shared.DTOs;
using ECommerceSystem.Shared.DTOs.Pet;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceSystem.Api.Controllers.Pet
{
    [ApiController]
    [Route("api/[controller]")]
    public class PetController : ControllerBase
    {
        private readonly IPetService _petService;

        public PetController(IPetService petService)
        {
            _petService = petService;
        }

        [HttpGet("new/{count:int}")]
        public async Task<ApiResult<PetListDto[]>> GetNewlyAddedPets(int count)
        {
            return await _petService.GetNewlyAddedPetsAsync(count);
        }

        [HttpGet("popular/{count:int}")]
        public async Task<ApiResult<PetListDto[]>> GetPopularPets(int count)
        {
            return await _petService.GetPopularPetsAsync(count);
        }

        [HttpGet]
        public async Task<ApiResult<PetListDto[]>> GetAllPets()
        {
            return await _petService.GetAllPetsAsync();
        }

        [HttpGet("random/{count:int}")]
        public async Task<ApiResult<PetListDto[]>> GetRandomPets(int count)
        {
            return await _petService.GetRandomPetsAsync(count);
        }

        [HttpGet("{id:int}")]
        public async Task<ApiResult<PetDetailDto>> GetPetDetails(int id)
        {
            return await _petService.GetPetDetailsAsync(id);
        }

        
    }
}
