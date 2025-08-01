using ECommerceSystem.GUI.Apis;
using ECommerceSystem.GUI.Models;
using ECommerceSystem.Shared.DTOs.Pet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceSystem.GUI.Controllers
{
    [Authorize]
    public class PetNewFeedController : Controller
    {
        private readonly IPetApi _petApi;
        private readonly IUserPetApi _userPetApi; // Thêm vào
        private const int DefaultPetDisplayCount = 6;

        // Cập nhật constructor để inject IUserPetApi
        public PetNewFeedController(IPetApi petApi, IUserPetApi userPetApi)
        {
            _petApi = petApi;
            _userPetApi = userPetApi;
        }

        public async Task<IActionResult> Index()
        {
            // Gọi thêm API lấy danh sách yêu thích
            var newPetsTask = _petApi.GetNewlyAddedPetsAsync(DefaultPetDisplayCount);
            var popularPetsTask = _petApi.GetPopularPetsAsync(DefaultPetDisplayCount);
            var randomPetsTask = _petApi.GetRandomPetsAsync(DefaultPetDisplayCount);
            var likedPetsTask = _userPetApi.GetUserFavoritesAsync(); // Gọi API mới

            // Đợi tất cả hoàn thành
            await Task.WhenAll(newPetsTask, popularPetsTask, randomPetsTask, likedPetsTask);

            // Lấy ra danh sách ID đã thích để tra cứu nhanh
            var likedIds = likedPetsTask.Result.Data?.Select(p => p.Id).ToHashSet() ?? new();

            // Hàm tiện ích để sắp xếp
            List<PetListDto> SortByLiked(IEnumerable<PetListDto> pets) =>
                pets.OrderByDescending(p => likedIds.Contains(p.Id)).ToList();

            var vm = new NewFeedViewModel
            {
                // Áp dụng sắp xếp cho từng danh sách
                NewPets = SortByLiked(newPetsTask.Result.Data ?? Enumerable.Empty<PetListDto>()),
                PopularPets = SortByLiked(popularPetsTask.Result.Data ?? Enumerable.Empty<PetListDto>()),
                RandomPets = SortByLiked(randomPetsTask.Result.Data ?? Enumerable.Empty<PetListDto>())
            };

            return View(vm);
        }
    }
}