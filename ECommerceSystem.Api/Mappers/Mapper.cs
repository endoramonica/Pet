using ECommerceSystem.Shared.DTOs.Pet;
using ECommerceSystem.Shared.Entities;

namespace ECommerceSystem.Api.Mappers
{
    public static class Mapper
    {
        public static PetListDto MapToPetListDto(this Pet pet) =>
    new PetListDto
    {
        Id = pet.Id,
        Name = pet.Name,
        Price = pet.Price,
        ImageUrl = pet.ImageUrl,
        PetType = pet.PetType,
        Location = pet.Location
        // Thêm các trường khác nếu cần
    };

        public static PetDetailDto MapToPetDetailDto(this Pet pet) =>
            new PetDetailDto
            {
                Id = pet.Id,
                Name = pet.Name,
                Price = pet.Price,
                Location = pet.Location,
                HealthStatus = pet.HealthStatus,
                Views = pet.Views,
                IsActive = pet.IsActive,
                Breed = pet.Breed,
                ImageUrl = pet.ImageUrl,
                PetType = pet.PetType,
                Description = pet.Description,
                AdoptionStatus = pet.AdoptionStatus,
                Gender = pet.Gender,
                DateOfBirth = pet.DateOfBirth, // ✅ sửa lại đúng trường
                // Các trường bên DTO như IsFavorite, AdoptionHistory, OwnerInfo, MedicalRecords
                // sẽ được xử lý ở tầng service nếu cần
            };
    }
}
