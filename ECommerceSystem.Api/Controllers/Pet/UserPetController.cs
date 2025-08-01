using ECommerceSystem.Api.Services;
using ECommerceSystem.Shared.DTOs;
using ECommerceSystem.Shared.DTOs.Pet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace ECommerceSystem.Api.Controllers.Pet
{
    [Route("api/user-pet-service")]
    [ApiController]
    public class UserPetController : ControllerBase
    {
        private readonly IUserPetService _userPetService;

        public UserPetController(IUserPetService userPetService)
        {
            _userPetService = userPetService;
        }
        private int UserId =>
            Convert.ToInt32(User.Claims.First(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Value);

        //adopt pet
        [HttpPost("adopt/{petId:int}")]
        public async Task<ApiResponseDto> AsyncUserAdoptionPet(int petId)
        {
            return await _userPetService.AdoptPetAsync(UserId, petId);
        }

        //toggle like pet
        [HttpPost("toggle-like/{petId:int}")]
        public async Task<ApiResult<bool>> ToggleLike(int petId)
        {
            return await _userPetService.ToggleLikeAsync(UserId, petId);
        }
        //get user favorites
        [HttpGet("favorites")]
        public async Task<ApiResult<PetListDto[]>> GetUserFavorites()
        {   
            return await _userPetService.GetUserFavoritesAsync(UserId);
        }
        
        //get user adoptions
        [HttpGet("adoptions")]
        public async Task<ApiResult<PetListDto[]>> GetUserAdoptions()
        {
            return await _userPetService.GetUserAdoptionsAsync(UserId);
        }


    }
}

