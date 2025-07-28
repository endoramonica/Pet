using ECommerceSystem.Shared.DTOs.Pet;
using ECommerceSystem.Shared.Entities;
using System.Linq.Expressions;
namespace ECommerceSystem.Api.Extension;
public static class Selector
{
    public static Expression<Func<Pet, PetListDto >>PetToPetListDto => 
        pet => new PetListDto
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
        PetType = pet.PetType
    };
    
}