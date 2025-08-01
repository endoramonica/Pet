using ECommerceSystem.GUI.Apis;
using ECommerceSystem.GUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceSystem.GUI.Controllers
{
    [Authorize]
    public class PetController : Controller
    {
        private readonly IPetApi _petApi;
        private readonly IUserPetApi _userPetApi;

        public PetController(IPetApi petApi, IUserPetApi userPetApi)
        {
            _petApi = petApi;
            _userPetApi = userPetApi;
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            // Gọi song song các API để tiết kiệm thời gian
            var petDetailTask = _petApi.GetPetDetailsAsync(id);
            var likedPetsTask = _userPetApi.GetUserFavoritesAsync();
            var adoptedPetsTask = _userPetApi.GetUserAdoptionsAsync();

            await Task.WhenAll(petDetailTask, likedPetsTask, adoptedPetsTask);

            var petResult = petDetailTask.Result;
            if (!petResult.Success|| petResult.Data == null)
            {
                return NotFound(); // Không tìm thấy thú cưng
            }

            // Ánh xạ thủ công từ DTO sang ViewModel
            var vm = new PetDetailViewModel
            {
                Id = petResult.Data.Id,
                Name = petResult.Data.Name,
                Description = petResult.Data.Description,
                Price = petResult.Data.Price,
                ImageUrl = petResult.Data.ImageUrl,
                // Kiểm tra xem ID của thú cưng này có trong danh sách thích/nhận nuôi không
                IsLiked = likedPetsTask.Result.Data?.Any(p => p.Id == id) ?? false,
                IsAdopted = adoptedPetsTask.Result.Data?.Any(p => p.Id == id) ?? false
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Thêm để bảo mật
        public async Task<IActionResult> ToggleLike(int id)
        {
            await _userPetApi.ToggleLikeAsync(id);
            // Chuyển hướng về trang Detail để cập nhật giao diện
            return RedirectToAction("Detail", new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Thêm để bảo mật
        public async Task<IActionResult> Adopt(int id)
        {
            await _userPetApi.AdoptPetAsync(id);
            // Chuyển hướng về trang Detail để cập nhật giao diện
            return RedirectToAction("Detail", new { id });
        }
    }
}