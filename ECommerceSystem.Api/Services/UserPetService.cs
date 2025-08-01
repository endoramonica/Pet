using ECommerceSystem.Api.Extension;
using ECommerceSystem.Shared.DTOs;
using ECommerceSystem.Shared.DTOs.Pet;
using ECommerceSystem.Shared.Entities;
using ECommerceSystem.Shared.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ECommerceSystem.Api.Services
{
    public interface IUserPetService
    {
        Task<ApiResult<bool>> ToggleLikeAsync(int userId, int petId);
        Task<ApiResult<PetListDto[]>> GetUserFavoritesAsync(int userId);
        Task<ApiResult<PetListDto[]>> GetUserAdoptionsAsync(int userId);
        Task<ApiResponseDto> AdoptPetAsync(int userId, int petId);
    }
    public class UserPetService : IUserPetService
    {
        private readonly SemaphoreSlim _semaphore = new(1, 1);
        private readonly WebDBContext _context;

        public UserPetService(WebDBContext webDBContext)
        {
            _context = webDBContext;
        }
        public async Task<ApiResult<bool>> ToggleLikeAsync(int userId, int petId)
        {
            var existing = await _context.UserFavorites
                .FirstOrDefaultAsync(x => x.UserId == userId && x.PetId == petId);

            if (existing is not null)
            {
                _context.UserFavorites.Remove(existing);
                await _context.SaveChangesAsync();
                return ApiResult<bool>.Ok(false, "Đã bỏ thích thú cưng.");
            }
            else
            {
                var newLike = new UserFavorite
                {
                    UserId = userId,
                    PetId = petId,
                    CreatedAt = DateTime.UtcNow
                };
                await _context.UserFavorites.AddAsync(newLike);
            }

            await _context.SaveChangesAsync();
            return ApiResult<bool>.Ok(true, "Đã thích thú cưng.");
        }
        public async Task<ApiResult<PetListDto[]>> GetUserFavoritesAsync(int userId)
        {
            var favorites = await _context.UserFavorites
                .Where(x => x.UserId == userId && !x.IsDeleted)
                .Select(x => x.Pet)
                .Select(Selector.PetToPetListDto)
                .ToArrayAsync();
            return ApiResult<PetListDto[]>.Ok(favorites, "Danh sách thú cưng yêu thích.");
        }
        public async Task<ApiResult<PetListDto[]>> GetUserAdoptionsAsync(int userId)
        {
            var adoptions = await _context.UserAdoptions
                .Where(x => x.UserId == userId)
                .Select(x => x.Pet)
                .Select(Selector.PetToPetListDto)
                .ToArrayAsync();
            return ApiResult<PetListDto[]>.Ok(adoptions, "Danh sách thú cưng đã nhận nuôi.");
        }
        public async Task<ApiResponseDto> AdoptPetAsync(int userId, int petId)
        {
            try
            {
                await _semaphore.WaitAsync();
                var pet = await _context.Pets.FirstOrDefaultAsync(p => p.Id == petId);
                if (pet == null)
                {
                    return ApiResponseDto.Fail("Thú cưng không tồn tại.");
                }
                if (pet.AdoptionStatus == AdoptionStatus.Adopted)
                {
                    return ApiResponseDto.Fail($"{pet.Name}Thú cưng đã được nhận nuôi.");
                }
                pet.AdoptionStatus = AdoptionStatus.Adopted;
                var adoption = new UserAdoption
                {
                    UserId = userId,
                    PetId = petId,
                    AdoptionDate = DateTime.UtcNow
                };
                await _context.UserAdoptions.AddAsync(adoption);
                await _context.SaveChangesAsync();
                return ApiResponseDto.Ok("Đã nhận nuôi thú cưng thành công.");
            }
            catch (Exception)
            {
                return ApiResponseDto.Fail("Đã xảy ra lỗi khi nhận nuôi thú cưng. Vui lòng thử lại sau.");

            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
