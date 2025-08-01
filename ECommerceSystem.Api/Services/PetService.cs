using ECommerceSystem.Api.Extension;
using ECommerceSystem.Api.Mappers;
using ECommerceSystem.Shared.DTOs;
using ECommerceSystem.Shared.DTOs.Pet;
using ECommerceSystem.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ECommerceSystem.Api.Services;
public interface IPetService
{
    Task<ApiResult<PetListDto[]>> GetNewlyAddedPetsAsync(int count);
    Task<ApiResult<PetListDto[]>> GetPopularPetsAsync(int count);
    Task<ApiResult<PetListDto[]>> GetAllPetsAsync();
    Task<ApiResult<PetListDto[]>> GetRandomPetsAsync(int count);
    Task<ApiResult<PetDetailDto>> GetPetDetailsAsync(int id);
    // --- Các phương thức mới cho Admin ---
    Task<ApiResult<PetListDto[]>> GetAllPetsForAdminAsync();
    Task<ApiResult<PetDetailDto>> GetPetByIdForAdminAsync(int petId);
    Task<ApiResult<PetCreateUpdateDto>> GetPetForUpdateAsync(int petId);
    Task<ApiResult<int>> CreatePetAsync(PetCreateUpdateDto petDto);
    Task<ApiResult<bool>> UpdatePetAsync(int petId, PetCreateUpdateDto petDto);
    Task<ApiResult<bool>> DeletePetAsync(int petId);

}
public class PetService : IPetService
{
    private readonly WebDBContext _db;

    public PetService(WebDBContext db)
    {
        _db = db;
    }

    public async Task<ApiResult<PetListDto[]>> GetNewlyAddedPetsAsync(int count)
    {
        var dtos = await _db.Pets
            .OrderByDescending(p => p.Id)
            .Take(count)
            .Select(Selector.PetToPetListDto)
            .ToArrayAsync();

        return ApiResult<PetListDto[]>.Ok(dtos);
    }

    public async Task<ApiResult<PetListDto[]>> GetPopularPetsAsync(int count)
    {
        var dtos = await _db.Pets
            .OrderByDescending(p => p.Views)
            .Take(count)
            .Select(Selector.PetToPetListDto)
            .ToArrayAsync();

        return ApiResult<PetListDto[]>.Ok(dtos);
    }

    public async Task<ApiResult<PetListDto[]>> GetAllPetsAsync()
    {
        var dtos = await _db.Pets
            .Select(Selector.PetToPetListDto)
            .ToArrayAsync();

        return ApiResult<PetListDto[]>.Ok(dtos);
    }

    public async Task<ApiResult<PetListDto[]>> GetRandomPetsAsync(int count)
    {
        var randomPets = await _db.Pets
     .OrderByDescending( _=> Guid.NewGuid()) // hoặc OrderBy(p => Guid.NewGuid()) nếu dùng SQLite
     .Take(count)
     .Select(Selector.PetToPetListDto)
     .ToArrayAsync();
        return ApiResult<PetListDto[]>.Ok(randomPets);
    }



    public async Task<ApiResult<PetDetailDto>> GetPetDetailsAsync(int id)
    {
        var pet = await _db.Pets.FirstOrDefaultAsync(p => p.Id == id);

        if (pet == null)
            return ApiResult<PetDetailDto>.Fail("Pet not found");

        // Tăng lượt xem
        pet.Views++;
        await _db.SaveChangesAsync();

        // Ánh xạ sang DTO
        var petDto = pet!.MapToPetDetailDto();

        return ApiResult<PetDetailDto>.Ok(petDto);
    }
    // --- Admin: Lấy toàn bộ thú cưng (bao gồm cả bị ẩn hoặc xóa nếu cần mở rộng sau này) ---
    public async Task<ApiResult<PetListDto[]>> GetAllPetsForAdminAsync()
    {
        var pets = await _db.Pets
            .OrderByDescending(p => p.Id)
            .Select(Selector.PetToPetListDto)
            .ToArrayAsync();

        return ApiResult<PetListDto[]>.Ok(pets);
    }

    // --- Admin: Lấy chi tiết thú cưng theo ID ---
    public async Task<ApiResult<PetDetailDto>> GetPetByIdForAdminAsync(int petId)
    {
        var pet = await _db.Pets.FindAsync(petId);
        if (pet == null)
            return ApiResult<PetDetailDto>.Fail("Pet not found");

        return ApiResult<PetDetailDto>.Ok(pet.MapToPetDetailDto());
    }

    // --- Admin: Lấy dữ liệu để cập nhật ---
    public async Task<ApiResult<PetCreateUpdateDto>> GetPetForUpdateAsync(int petId)
    {
        var pet = await _db.Pets.FindAsync(petId);
        if (pet == null)
            return ApiResult<PetCreateUpdateDto>.Fail("Pet not found");

        var dto = new PetCreateUpdateDto
        {
            Name = pet.Name,
            Description = pet.Description,
            DateOfBirth = pet.DateOfBirth,
            PetType = pet.PetType,
            Gender = pet.Gender,
            Price = pet.Price,
            ImageUrl = pet.ImageUrl,

            Breed = pet.Breed,
            Location = pet.Location,
            HealthStatus = pet.HealthStatus
        };

        return ApiResult<PetCreateUpdateDto>.Ok(dto);
    }

    // --- Admin: Tạo mới thú cưng ---
    public async Task<ApiResult<int>> CreatePetAsync(PetCreateUpdateDto petDto)
    {
        var pet = new Pet
        {
            Name = petDto.Name,
            Description = petDto.Description,
            DateOfBirth = petDto.DateOfBirth,
            PetType = petDto.PetType,
            Gender = petDto.Gender,
            Price = petDto.Price,
            ImageUrl = petDto.ImageUrl,
            Breed = petDto.Breed,
            Location = petDto.Location,
            HealthStatus = petDto.HealthStatus,


            Views = 0
        };

        _db.Pets.Add(pet);
        await _db.SaveChangesAsync();

        return ApiResult<int>.Ok(pet.Id);
    }

    // --- Admin: Cập nhật thú cưng ---
    public async Task<ApiResult<bool>> UpdatePetAsync(int petId, PetCreateUpdateDto petDto)
    {
        var pet = await _db.Pets.FindAsync(petId);
        if (pet == null)
            return ApiResult<bool>.Fail("Pet not found");

        pet.Name = petDto.Name;
        pet.Description = petDto.Description;
        pet.DateOfBirth = petDto.DateOfBirth;
        pet.PetType = petDto.PetType;
        pet.Gender = petDto.Gender;
        pet.Price = petDto.Price;
        pet.ImageUrl = petDto.ImageUrl;

        pet.Breed = petDto.Breed;
        pet.Location = petDto.Location;
        pet.HealthStatus = petDto.HealthStatus;
        await _db.SaveChangesAsync();
        return ApiResult<bool>.Ok(true);
    }

    // --- Admin: Xóa thú cưng ---
    public async Task<ApiResult<bool>> DeletePetAsync(int petId)
    {
        var pet = await _db.Pets.FindAsync(petId);
        if (pet == null)
            return ApiResult<bool>.Fail("Pet not found");

        _db.Pets.Remove(pet);
        await _db.SaveChangesAsync();
        return ApiResult<bool>.Ok(true);
    }

}
