using ECommerceSystem.GUI.Apis;
using ECommerceSystem.GUI.Models;
using ECommerceSystem.Shared.DTOs.Pet;
using ECommerceSystem.Shared.Enums;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.IO;
using System.Xml;
using Newtonsoft.Json;

namespace ECommerceSystem.AdminWeb.Controllers;

[Route("admin/pets")]
public class AdminPetController : Controller
{
    private readonly IPetApi _petApi;
    private readonly IWebHostEnvironment _env;

    public AdminPetController(IPetApi petApi, IWebHostEnvironment env)
    {
        _petApi = petApi;
        _env = env;
    }

    // GET: /admin/pets
    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var result = await _petApi.GetAllPetsAsync();
        return View(result.Data);
    }

    // GET: /admin/pets/create
    [HttpGet("create")]
    public IActionResult Create()
    {
        return View(new PetDetailViewModel());
    }

    // POST: /admin/pets/create
    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PetDetailViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);

        if (vm.ImageFile != null)
        {
            vm.ImageUrl = await SaveImage(vm.ImageFile);
        }

        var dto = new PetCreateUpdateDto
        {
            Name = vm.Name,
            Price = vm.Price,
            Location = vm.Location,
            HealthStatus = vm.HealthStatus,
            Breed = vm.Breed,
            Description = vm.Description,
            ImageUrl = vm.ImageUrl,
            DateOfBirth = vm.DateOfBirth,
            Gender = vm.Gender,
            PetType = vm.PetType
        };

        var result = await _petApi.CreatePetAsync(dto);
        if (result.Success) return RedirectToAction(nameof(Index));

        ModelState.AddModelError(string.Empty, result.Message ?? "Đã có lỗi xảy ra.");
        return View(vm);
    }

    // GET: /admin/pets/edit/{id}
    [HttpGet("edit/{id:int}")]
    public async Task<IActionResult> Edit(int id)
    {
        var result = await _petApi.GetPetForUpdateAsync(id);
        if (!result.Success) return NotFound();

        var pet = result.Data;

        var vm = new PetDetailViewModel
        {
            Id = id,
            Name = pet.Name,
            Price = pet.Price,
            Location = pet.Location,
            HealthStatus = pet.HealthStatus,
            Breed = pet.Breed,
            Description = pet.Description,
            ImageUrl = pet.ImageUrl,
            DateOfBirth = pet.DateOfBirth,
            Gender = pet.Gender,
            PetType = pet.PetType
        };

        return View(vm);
    }

    // POST: /admin/pets/edit/{id}
    [HttpPost("edit/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, PetDetailViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);

        if (vm.ImageFile != null)
        {
            if (!string.IsNullOrEmpty(vm.ImageUrl)) DeleteImage(vm.ImageUrl);
            vm.ImageUrl = await SaveImage(vm.ImageFile);
        }

        var dto = new PetCreateUpdateDto
        {
            Name = vm.Name,
            Price = vm.Price,
            Location = vm.Location,
            HealthStatus = vm.HealthStatus,
            Breed = vm.Breed,
            Description = vm.Description,
            ImageUrl = vm.ImageUrl,
            DateOfBirth = vm.DateOfBirth,
            Gender = vm.Gender,
            PetType = vm.PetType
        };
        System.Diagnostics.Debug.WriteLine("DTO gửi lên: " + Newtonsoft.Json.JsonConvert.SerializeObject(dto));
        var result = await _petApi.UpdatePetAsync(id, dto);
        if (result.Success) return RedirectToAction(nameof(Index));

        ModelState.AddModelError(string.Empty, result.Message ?? "Lỗi khi cập nhật.");
        return View(vm);
    }

    // POST: /admin/pets/delete/{id}
    [HttpPost("delete/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _petApi.DeletePetAsync(id);
        if (!result.Success)
        {
            TempData["Error"] = result.Message ?? "Xóa thất bại.";
        }
        return RedirectToAction(nameof(Index));
    }

    private async Task<string> SaveImage(IFormFile file)
    {
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var filePath = Path.Combine(_env.WebRootPath, "uploads", "pets", fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return $"/uploads/pets/{fileName}";
    }

    private void DeleteImage(string imageUrl)
    {
        var path = Path.Combine(_env.WebRootPath, imageUrl.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));
        if (System.IO.File.Exists(path))
        {
            System.IO.File.Delete(path);
        }
    }
}
