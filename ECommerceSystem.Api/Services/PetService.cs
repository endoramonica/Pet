using ECommerceSystem.Api.Data;
using ECommerceSystem.Shared.DTOs.Pet;
using ECommerceSystem.Shared.Entities;
using ECommerceSystem.Shared.DTOs;
using Microsoft.EntityFrameworkCore;
using ECommerceSystem.Api.Extension;
using ECommerceSystem.Api.Mappers;
public interface IPetService
{
    Task<ApiResult<PetListDto[]>> GetNewlyAddedPetsAsync(int count);
    Task<ApiResult<PetListDto[]>> GetPopularPetsAsync(int count);
    Task<ApiResult<PetListDto[]>> GetAllPetsAsync();
    Task<ApiResult<PetDetailDto>> GetPetDetailsAsync(int id);
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

    

    public async Task<ApiResult<PetDetailDto>> GetPetDetailsAsync(int id)
    {
        var petDto = await _db.Pets
            .Where(p => p.Id == id)
            .Select(pet => new PetDetailDto
            {
                Id = pet.Id,
                Name = pet.Name,
                Price = pet.Price,
                Location = pet.Location,
                HealthStatus = pet.HealthStatus,
                Views = pet.Views,
                IsActive = pet.IsActive,
                Breed = pet.Breed,
               DateOfBirth = pet.DateOfBirth,
                ImageUrl = pet.ImageUrl,
                Description = pet.Description,
                AdoptionStatus = pet.AdoptionStatus,
                Gender = pet.Gender
            })
            .FirstOrDefaultAsync();

        if (petDto == null)
            return ApiResult<PetDetailDto>.Fail("Pet not found");

        return ApiResult<PetDetailDto>.Ok(petDto);
    }
}
